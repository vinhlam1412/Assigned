import { mapEnumToOptions } from '@abp/ng.core';

export enum PriorityAssign {
  Low = 1,
  Medium = 2,
  High = 3,
  Urgent = 4,
}

export const priorityAssignOptions = mapEnumToOptions(PriorityAssign);
