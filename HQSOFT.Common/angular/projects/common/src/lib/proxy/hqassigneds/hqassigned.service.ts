import type { GetHQAssignedsInput, HQAssignedCreateDto, HQAssignedDto, HQAssignedExcelDownloadDto, HQAssignedUpdateDto, HQAssignedWithNavigationPropertiesDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { DownloadTokenResultDto, LookupDto, LookupRequestDto } from '../shared/models';

@Injectable({
  providedIn: 'root',
})
export class HQAssignedService {
  apiName = 'Common';
  

  create = (input: HQAssignedCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HQAssignedDto>({
      method: 'POST',
      url: '/api/common/hqassigneds',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/common/hqassigneds/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HQAssignedDto>({
      method: 'GET',
      url: `/api/common/hqassigneds/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getDownloadToken = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DownloadTokenResultDto>({
      method: 'GET',
      url: '/api/common/hqassigneds/download-token',
    },
    { apiName: this.apiName,...config });
  

  getIdentityUserLookup = (input: LookupRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<LookupDto<string>>>({
      method: 'GET',
      url: '/api/common/hqassigneds/identity-user-lookup',
      params: { filter: input.filter, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetHQAssignedsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<HQAssignedWithNavigationPropertiesDto>>({
      method: 'GET',
      url: '/api/common/hqassigneds',
      params: { filterText: input.filterText, idParent: input.idParent, completebyMin: input.completebyMin, completebyMax: input.completebyMax, priority: input.priority, comment: input.comment, identityUserId: input.identityUserId, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getListAsExcelFile = (input: HQAssignedExcelDownloadDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>({
      method: 'GET',
      responseType: 'blob',
      url: '/api/common/hqassigneds/as-excel-file',
      params: { downloadToken: input.downloadToken, filterText: input.filterText, name: input.name },
    },
    { apiName: this.apiName,...config });
  

  getWithNavigationProperties = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HQAssignedWithNavigationPropertiesDto>({
      method: 'GET',
      url: `/api/common/hqassigneds/with-navigation-properties/${id}`,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: HQAssignedUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HQAssignedDto>({
      method: 'PUT',
      url: `/api/common/hqassigneds/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
