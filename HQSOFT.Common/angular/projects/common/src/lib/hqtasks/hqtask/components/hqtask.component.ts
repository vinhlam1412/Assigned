import { ABP, downloadBlob, ListService, PagedResultDto, TrackByService } from '@abp/ng.core';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { DateAdapter } from '@abp/ng.theme.shared/extensions';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { filter, finalize, switchMap, tap } from 'rxjs/operators';
import { statusTaskOptions } from '../../../proxy/hqsoft/configuration/hqtasks/status-task.enum';
import { priorityTaskOptions } from '../../../proxy/hqsoft/configuration/hqtasks/priority-task.enum';
import type { GetHQTasksInput, HQTaskDto } from '../../../proxy/hqtasks/models';
import { HQTaskService } from '../../../proxy/hqtasks/hqtask.service';
@Component({
  selector: 'lib-hqtask',
  changeDetection: ChangeDetectionStrategy.Default,
  providers: [ListService, { provide: NgbDateAdapter, useClass: DateAdapter }],
  templateUrl: './hqtask.component.html',
  styles: [],
})
export class HQTaskComponent implements OnInit {
  data: PagedResultDto<HQTaskDto> = {
    items: [],
    totalCount: 0,
  };

  filters = {} as GetHQTasksInput;

  form: FormGroup;

  isFiltersHidden = true;

  isModalBusy = false;

  isModalOpen = false;

  isExportToExcelBusy = false;

  selected?: HQTaskDto;

  statusTaskOptions = statusTaskOptions;

  priorityTaskOptions = priorityTaskOptions;

  constructor(
    public readonly list: ListService,
    public readonly track: TrackByService,
    public readonly service: HQTaskService,
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

    const setData = (list: PagedResultDto<HQTaskDto>) => (this.data = list);

    this.list.hookToQuery(getData).subscribe(setData);
  }

  clearFilters() {
    this.filters = {} as GetHQTasksInput;
  }

  buildForm() {
    const {
      subject,
      project,
      status,
      priority,
      expectedStartDate,
      expectedEndDate,
      expectedTime,
      process,
    } = this.selected || {};

    this.form = this.fb.group({
      subject: [subject ?? null, [Validators.required]],
      project: [project ?? null, [Validators.required]],
      status: [status ?? null, []],
      priority: [priority ?? null, []],
      expectedStartDate: [expectedStartDate ? new Date(expectedStartDate) : null, []],
      expectedEndDate: [expectedEndDate ? new Date(expectedEndDate) : null, []],
      expectedTime: [expectedTime ?? null, []],
      process: [process ?? null, []],
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
      ? this.service.update(this.selected.id, {
          ...this.form.value,
          concurrencyStamp: this.selected.concurrencyStamp,
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

  update(record: HQTaskDto) {
    this.selected = record;
    this.showForm();
  }

  delete(record: HQTaskDto) {
    this.confirmation
      .warn('Common::DeleteConfirmationMessage', 'Common::AreYouSure', {
        messageLocalizationParams: [],
      })
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() => this.service.delete(record.id))
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
        downloadBlob(result, 'HQTask.xlsx');
      });
  }
}
