import type { GetHQTasksInput, HQTaskCreateDto, HQTaskDto, HQTaskExcelDownloadDto, HQTaskUpdateDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { DownloadTokenResultDto } from '../shared/models';

@Injectable({
  providedIn: 'root',
})
export class HQTaskService {
  apiName = 'Common';
  

  create = (input: HQTaskCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HQTaskDto>({
      method: 'POST',
      url: '/api/common/hqtasks',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/common/hqtasks/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HQTaskDto>({
      method: 'GET',
      url: `/api/common/hqtasks/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getDownloadToken = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DownloadTokenResultDto>({
      method: 'GET',
      url: '/api/common/hqtasks/download-token',
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetHQTasksInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<HQTaskDto>>({
      method: 'GET',
      url: '/api/common/hqtasks',
      params: { filterText: input.filterText, subject: input.subject, project: input.project, status: input.status, priority: input.priority, expectedStartDateMin: input.expectedStartDateMin, expectedStartDateMax: input.expectedStartDateMax, expectedEndDateMin: input.expectedEndDateMin, expectedEndDateMax: input.expectedEndDateMax, expectedTimeMin: input.expectedTimeMin, expectedTimeMax: input.expectedTimeMax, processMin: input.processMin, processMax: input.processMax, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getListAsExcelFile = (input: HQTaskExcelDownloadDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>({
      method: 'GET',
      responseType: 'blob',
      url: '/api/common/hqtasks/as-excel-file',
      params: { downloadToken: input.downloadToken, filterText: input.filterText, name: input.name },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: HQTaskUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HQTaskDto>({
      method: 'PUT',
      url: `/api/common/hqtasks/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
