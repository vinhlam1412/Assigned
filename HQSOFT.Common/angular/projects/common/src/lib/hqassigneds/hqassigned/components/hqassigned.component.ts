import { ABP, downloadBlob, ListService, PagedResultDto, TrackByService } from '@abp/ng.core';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { DateAdapter } from '@abp/ng.theme.shared/extensions';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { filter, finalize, switchMap, tap } from 'rxjs/operators';
import { priorityAssignOptions } from '../../../proxy/hqsoft/configuration/hqassigments/priority-assign.enum';
import type {
  GetHQAssignedsInput,
  HQAssignedWithNavigationPropertiesDto,
} from '../../../proxy/hqassigneds/models';
import { HQAssignedService } from '../../../proxy/hqassigneds/hqassigned.service';
@Component({
  selector: 'lib-hqassigned',
  changeDetection: ChangeDetectionStrategy.Default,
  providers: [ListService, { provide: NgbDateAdapter, useClass: DateAdapter }],
  templateUrl: './hqassigned.component.html',
  styles: [],
})
export class HQAssignedComponent implements OnInit {
  data: PagedResultDto<HQAssignedWithNavigationPropertiesDto> = {
    items: [],
    totalCount: 0,
  };

  filters = {} as GetHQAssignedsInput;

  form: FormGroup;

  isFiltersHidden = true;

  isModalBusy = false;

  isModalOpen = false;

  isExportToExcelBusy = false;

  selected?: HQAssignedWithNavigationPropertiesDto;

  priorityAssignOptions = priorityAssignOptions;

  constructor(
    public readonly list: ListService,
    public readonly track: TrackByService,
    public readonly service: HQAssignedService,
    private confirmation: ConfirmationService,
    private fb: FormBuilder
  ) {}

  ngOnInit() {
    const getData = (query: ABP.PageQueryParams) =>
      this.service.getList({
        ...query,
        ...this.filters,
        filterText: query.filter,
      });

    const setData = (list: PagedResultDto<HQAssignedWithNavigationPropertiesDto>) =>
      (this.data = list);

    this.list.hookToQuery(getData).subscribe(setData);
  }

  clearFilters() {
    this.filters = {} as GetHQAssignedsInput;
  }

  buildForm() {
    const { idParent, completeby, priority, comment } = this.selected?.hqAssigned || {};

    const { identityUsers = [] } = this.selected || {};

    this.form = this.fb.group({
      idParent: [idParent ?? null, []],
      completeby: [completeby ? new Date(completeby) : null, []],
      priority: [priority ?? null, []],
      comment: [comment ?? null, []],
      identityUserIds: [identityUsers.map(({ id }) => id), []],
    });
  }

  hideForm() {
    this.isModalOpen = false;
    this.form.reset();
  }

  showForm() {
    this.buildForm();
    this.isModalOpen = true;
  }

  submitForm() {
    if (this.form.invalid) return;

    const request = this.selected
      ? this.service.update(this.selected.hqAssigned.id, {
          ...this.form.value,
          concurrencyStamp: this.selected.hqAssigned.concurrencyStamp,
        })
      : this.service.create(this.form.value);

    this.isModalBusy = true;

    request
      .pipe(
        finalize(() => (this.isModalBusy = false)),
        tap(() => this.hideForm())
      )
      .subscribe(this.list.get);
  }

  create() {
    this.selected = undefined;
    this.showForm();
  }

  update(record: HQAssignedWithNavigationPropertiesDto) {
    this.service.getWithNavigationProperties(record.hqAssigned.id).subscribe(data => {
      this.selected = data;
      this.showForm();
    });
  }

  delete(record: HQAssignedWithNavigationPropertiesDto) {
    this.confirmation
      .warn('Common::DeleteConfirmationMessage', 'Common::AreYouSure', {
        messageLocalizationParams: [],
      })
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() => this.service.delete(record.hqAssigned.id))
      )
      .subscribe(this.list.get);
  }

  exportToExcel() {
    this.isExportToExcelBusy = true;
    this.service
      .getDownloadToken()
      .pipe(
        switchMap(({ token }) =>
          this.service.getListAsExcelFile({ downloadToken: token, filterText: this.list.filter })
        ),
        finalize(() => (this.isExportToExcelBusy = false))
      )
      .subscribe(result => {
        downloadBlob(result, 'HQAssigned.xlsx');
      });
  }
}
