import type { FullAuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { IdentityUserDto } from '../volo/abp/identity/models';

export interface GetHQSharesInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  idParent?: string;
  canRead?: boolean;
  canWrite?: boolean;
  canSubmit?: boolean;
  canShare?: boolean;
  identityUserId?: string;
}

export interface HQShareCreateDto {
  idParent?: string;
  canRead?: boolean;
  canWrite?: boolean;
  canSubmit?: boolean;
  canShare?: boolean;
  identityUserId?: string;
}

export interface HQShareDto extends FullAuditedEntityDto<string> {
  idParent?: string;
  canRead?: boolean;
  canWrite?: boolean;
  canSubmit?: boolean;
  canShare?: boolean;
  identityUserId?: string;
  concurrencyStamp?: string;
}

export interface HQShareExcelDownloadDto {
  downloadToken?: string;
  filterText?: string;
  name?: string;
}

export interface HQShareUpdateDto {
  idParent?: string;
  canRead?: boolean;
  canWrite?: boolean;
  canSubmit?: boolean;
  canShare?: boolean;
  identityUserId?: string;
  concurrencyStamp?: string;
}

export interface HQShareWithNavigationPropertiesDto {
  hqShare: HQShareDto;
  identityUser: IdentityUserDto;
}
