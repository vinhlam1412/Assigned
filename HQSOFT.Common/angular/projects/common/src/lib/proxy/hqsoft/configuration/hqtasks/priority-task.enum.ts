import { mapEnumToOptions } from '@abp/ng.core';

export enum PriorityTask {
  Low = 1,
  Medium = 2,
  High = 3,
  Urgent = 4,
}

export const priorityTaskOptions = mapEnumToOptions(PriorityTask);
