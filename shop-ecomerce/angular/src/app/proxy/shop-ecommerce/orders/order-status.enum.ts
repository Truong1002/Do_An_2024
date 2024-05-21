import { mapEnumToOptions } from '@abp/ng.core';

export enum OrderStatus {
  New = 1,
  Confirmed = 2,
  Processing = 3,
  Shipping = 4,
  Finished = 5,
  Canceled = 6,
}

export const orderStatusOptions = mapEnumToOptions(OrderStatus);
