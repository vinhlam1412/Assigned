import type { GetHQNotificationsInput, HQNotificationCreateDto, HQNotificationDto, HQNotificationExcelDownloadDto, HQNotificationUpdateDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { DownloadTokenResultDto } from '../shared/models';

@Injectable({
  providedIn: 'root',
})
export class HQNotificationService {
  apiName = 'Common';
  

  create = (input: HQNotificationCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HQNotificationDto>({
      method: 'POST',
      url: '/api/common/hqnotifications',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/common/hqnotifications/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HQNotificationDto>({
      method: 'GET',
      url: `/api/common/hqnotifications/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getDownloadToken = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DownloadTokenResultDto>({
      method: 'GET',
      url: '/api/common/hqnotifications/download-token',
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetHQNotificationsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<HQNotificationDto>>({
      method: 'GET',
      url: '/api/common/hqnotifications',
      params: { filterText: input.filterText, idParent: input.idParent, toUser: input.toUser, fromUser: input.fromUser, notiTitle: input.notiTitle, notiBody: input.notiBody, type: input.type, isRead: input.isRead, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getListAsExcelFile = (input: HQNotificationExcelDownloadDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>({
      method: 'GET',
      responseType: 'blob',
      url: '/api/common/hqnotifications/as-excel-file',
      params: { downloadToken: input.downloadToken, filterText: input.filterText, name: input.name },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: HQNotificationUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HQNotificationDto>({
      method: 'PUT',
      url: `/api/common/hqnotifications/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
