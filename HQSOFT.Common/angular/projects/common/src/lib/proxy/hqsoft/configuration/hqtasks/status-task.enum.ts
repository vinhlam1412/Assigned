import { mapEnumToOptions } from '@abp/ng.core';

export enum StatusTask {
  Open = 1,
  Working = 2,
  Pendding = 3,
  Completed = 4,
  Overdue = 5,
}

export const statusTaskOptions = mapEnumToOptions(StatusTask);
