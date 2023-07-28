import type { FullAuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetHQNotificationsInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  idParent?: string;
  toUser?: string;
  fromUser?: string;
  notiTitle?: string;
  notiBody?: string;
  type?: string;
  isRead?: boolean;
}

export interface HQNotificationCreateDto {
  idParent?: string;
  toUser?: string;
  fromUser?: string;
  notiTitle?: string;
  notiBody?: string;
  type?: string;
  isRead?: boolean;
}

export interface HQNotificationDto extends FullAuditedEntityDto<string> {
  idParent?: string;
  toUser?: string;
  fromUser?: string;
  notiTitle?: string;
  notiBody?: string;
  type?: string;
  isRead?: boolean;
  concurrencyStamp?: string;
}

export interface HQNotificationExcelDownloadDto {
  downloadToken?: string;
  filterText?: string;
  name?: string;
}

export interface HQNotificationUpdateDto {
  idParent?: string;
  toUser?: string;
  fromUser?: string;
  notiTitle?: string;
  notiBody?: string;
  type?: string;
  isRead?: boolean;
  concurrencyStamp?: string;
}
