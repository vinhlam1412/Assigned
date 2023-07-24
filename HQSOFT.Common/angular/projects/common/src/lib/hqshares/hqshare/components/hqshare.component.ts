import { ABP, downloadBlob, ListService, PagedResultDto, TrackByService } from '@abp/ng.core';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { DateAdapter } from '@abp/ng.theme.shared/extensions';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { filter, finalize, switchMap, tap } from 'rxjs/operators';
import type {
  GetHQSharesInput,
  HQShareWithNavigationPropertiesDto,
} from '../../../proxy/hqshares/models';
import { HQShareService } from '../../../proxy/hqshares/hqshare.service';
@Component({
  selector: 'lib-hqshare',
  changeDetection: ChangeDetectionStrategy.Default,
  providers: [ListService, { provide: NgbDateAdapter, useClass: DateAdapter }],
  templateUrl: './hqshare.component.html',
  styles: [],
})
export class HQShareComponent implements OnInit {
  data: PagedResultDto<HQShareWithNavigationPropertiesDto> = {
    items: [],
    totalCount: 0,
  };

  filters = {} as GetHQSharesInput;

  form: FormGroup;

  isFiltersHidden = true;

  isModalBusy = false;

  isModalOpen = false;

  isExportToExcelBusy = false;

  selected?: HQShareWithNavigationPropertiesDto;

  constructor(
    public readonly list: ListService,
    public readonly track: TrackByService,
    public readonly service: HQShareService,
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

    const setData = (list: PagedResultDto<HQShareWithNavigationPropertiesDto>) =>
      (this.data = list);

    this.list.hookToQuery(getData).subscribe(setData);
  }

  clearFilters() {
    this.filters = {} as GetHQSharesInput;
  }

  buildForm() {
    const { idParent, canRead, canWrite, canSubmit, canShare } = this.selected?.hqShare || {};

    const { identityUsers = [] } = this.selected || {};

    this.form = this.fb.group({
      idParent: [idParent ?? null, []],
      canRead: [canRead ?? false, []],
      canWrite: [canWrite ?? false, []],
      canSubmit: [canSubmit ?? false, []],
      canShare: [canShare ?? false, []],
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
      ? this.service.update(this.selected.hqShare.id, {
          ...this.form.value,
          concurrencyStamp: this.selected.hqShare.concurrencyStamp,
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

  update(record: HQShareWithNavigationPropertiesDto) {
    this.service.getWithNavigationProperties(record.hqShare.id).subscribe(data => {
      this.selected = data;
      this.showForm();
    });
  }

  delete(record: HQShareWithNavigationPropertiesDto) {
    this.confirmation
      .warn('Common::DeleteConfirmationMessage', 'Common::AreYouSure', {
        messageLocalizationParams: [],
      })
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() => this.service.delete(record.hqShare.id))
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
        downloadBlob(result, 'HQShare.xlsx');
      });
  }
}
