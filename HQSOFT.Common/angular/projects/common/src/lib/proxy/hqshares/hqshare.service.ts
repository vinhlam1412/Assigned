import type { GetHQSharesInput, HQShareCreateDto, HQShareDto, HQShareExcelDownloadDto, HQShareUpdateDto, HQShareWithNavigationPropertiesDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { DownloadTokenResultDto, LookupDto, LookupRequestDto } from '../shared/models';

@Injectable({
  providedIn: 'root',
})
export class HQShareService {
  apiName = 'Common';
  

  create = (input: HQShareCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HQShareDto>({
      method: 'POST',
      url: '/api/common/hqshares',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/common/hqshares/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HQShareDto>({
      method: 'GET',
      url: `/api/common/hqshares/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getDownloadToken = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DownloadTokenResultDto>({
      method: 'GET',
      url: '/api/common/hqshares/download-token',
    },
    { apiName: this.apiName,...config });
  

  getIdentityUserLookup = (input: LookupRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<LookupDto<string>>>({
      method: 'GET',
      url: '/api/common/hqshares/identity-user-lookup',
      params: { filter: input.filter, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetHQSharesInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<HQShareWithNavigationPropertiesDto>>({
      method: 'GET',
      url: '/api/common/hqshares',
      params: { filterText: input.filterText, idParent: input.idParent, canRead: input.canRead, canWrite: input.canWrite, canSubmit: input.canSubmit, canShare: input.canShare, identityUserId: input.identityUserId, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getListAsExcelFile = (input: HQShareExcelDownloadDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>({
      method: 'GET',
      responseType: 'blob',
      url: '/api/common/hqshares/as-excel-file',
      params: { downloadToken: input.downloadToken, filterText: input.filterText, name: input.name },
    },
    { apiName: this.apiName,...config });
  

  getWithNavigationProperties = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HQShareWithNavigationPropertiesDto>({
      method: 'GET',
      url: `/api/common/hqshares/with-navigation-properties/${id}`,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: HQShareUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HQShareDto>({
      method: 'PUT',
      url: `/api/common/hqshares/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
