import { mapEnumToOptions } from '@abp/ng.core';

export enum PaymentMethod {
  COD = 1,
  OnlinePayment = 2,
  TransferByBank = 3,
  CreditCard = 4,
}

export const paymentMethodOptions = mapEnumToOptions(PaymentMethod);
