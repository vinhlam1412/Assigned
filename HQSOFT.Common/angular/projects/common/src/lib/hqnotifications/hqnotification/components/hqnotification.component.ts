import { ABP, downloadBlob, ListService, PagedResultDto, TrackByService } from '@abp/ng.core';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { DateAdapter } from '@abp/ng.theme.shared/extensions';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { filter, finalize, switchMap, tap } from 'rxjs/operators';
import type {
  GetHQNotificationsInput,
  HQNotificationDto,
} from '../../../proxy/hqnotifications/models';
import { HQNotificationService } from '../../../proxy/hqnotifications/hqnotification.service';
@Component({
  selector: 'lib-hqnotification',
  changeDetection: ChangeDetectionStrategy.Default,
  providers: [ListService, { provide: NgbDateAdapter, useClass: DateAdapter }],
  templateUrl: './hqnotification.component.html',
  styles: [],
})
export class HQNotificationComponent implements OnInit {
  data: PagedResultDto<HQNotificationDto> = {
    items: [],
    totalCount: 0,
  };

  filters = {} as GetHQNotificationsInput;

  form: FormGroup;

  isFiltersHidden = true;

  isModalBusy = false;

  isModalOpen = false;

  isExportToExcelBusy = false;

  selected?: HQNotificationDto;

  constructor(
    public readonly list: ListService,
    public readonly track: TrackByService,
    public readonly service: HQNotificationService,
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

    const setData = (list: PagedResultDto<HQNotificationDto>) => (this.data = list);

    this.list.hookToQuery(getData).subscribe(setData);
  }

  clearFilters() {
    this.filters = {} as GetHQNotificationsInput;
  }

  buildForm() {
    const { idParent, toUser, fromUser, notiTitle, notiBody, type, isRead } = this.selected || {};

    this.form = this.fb.group({
      idParent: [idParent ?? null, []],
      toUser: [toUser ?? null, []],
      fromUser: [fromUser ?? null, []],
      notiTitle: [notiTitle ?? null, []],
      notiBody: [notiBody ?? null, []],
      type: [type ?? null, []],
      isRead: [isRead ?? false, []],
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

  update(record: HQNotificationDto) {
    this.selected = record;
    this.showForm();
  }

  delete(record: HQNotificationDto) {
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
        downloadBlob(result, 'HQNotification.xlsx');
      });
  }
}
