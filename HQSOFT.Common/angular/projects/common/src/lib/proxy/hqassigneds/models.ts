import type { FullAuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { PriorityAssign } from '../hqsoft/configuration/hqassigments/priority-assign.enum';
import type { IdentityUserDto } from '../volo/abp/identity/models';

export interface GetHQAssignedsInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  idParent?: string;
  completebyMin?: string;
  completebyMax?: string;
  priority?: PriorityAssign;
  comment?: string;
  identityUserId?: string;
}

export interface HQAssignedCreateDto {
  idParent?: string;
  completeby?: string;
  priority?: PriorityAssign;
  comment?: string;
  identityUserIds: string[];
}

export interface HQAssignedDto extends FullAuditedEntityDto<string> {
  idParent?: string;
  completeby?: string;
  priority?: PriorityAssign;
  comment?: string;
  concurrencyStamp?: string;
}

export interface HQAssignedExcelDownloadDto {
  downloadToken?: string;
  filterText?: string;
  name?: string;
}

export interface HQAssignedUpdateDto {
  idParent?: string;
  completeby?: string;
  priority?: PriorityAssign;
  comment?: string;
  identityUserIds: string[];
  concurrencyStamp?: string;
}

export interface HQAssignedWithNavigationPropertiesDto {
  hqAssigned: HQAssignedDto;
  identityUsers: IdentityUserDto[];
}
