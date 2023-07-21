import type { FullAuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { StatusTask } from '../hqsoft/configuration/hqtasks/status-task.enum';
import type { PriorityTask } from '../hqsoft/configuration/hqtasks/priority-task.enum';

export interface GetHQTasksInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  subject?: string;
  project?: string;
  status?: StatusTask;
  priority?: PriorityTask;
  expectedStartDateMin?: string;
  expectedStartDateMax?: string;
  expectedEndDateMin?: string;
  expectedEndDateMax?: string;
  expectedTimeMin?: number;
  expectedTimeMax?: number;
  processMin?: number;
  processMax?: number;
}

export interface HQTaskCreateDto {
  subject: string;
  project: string;
  status?: StatusTask;
  priority?: PriorityTask;
  expectedStartDate?: string;
  expectedEndDate?: string;
  expectedTime?: number;
  process?: number;
}

export interface HQTaskDto extends FullAuditedEntityDto<string> {
  subject: string;
  project: string;
  status?: StatusTask;
  priority?: PriorityTask;
  expectedStartDate?: string;
  expectedEndDate?: string;
  expectedTime?: number;
  process?: number;
  concurrencyStamp?: string;
}

export interface HQTaskExcelDownloadDto {
  downloadToken?: string;
  filterText?: string;
  name?: string;
}

export interface HQTaskUpdateDto {
  subject: string;
  project: string;
  status?: StatusTask;
  priority?: PriorityTask;
  expectedStartDate?: string;
  expectedEndDate?: string;
  expectedTime?: number;
  process?: number;
  concurrencyStamp?: string;
}
