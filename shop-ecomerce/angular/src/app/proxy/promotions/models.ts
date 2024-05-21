import type { EntityDto } from '@abp/ng.core';

export interface CreatePromotionDto {
  name?: string;
  couponCode?: string;
  requireUseCouponCode: boolean;
  validDate?: string;
  expiredDate?: string;
  discountAmount: number;
  limitedUsageTimes: boolean;
  maximumDiscountAmount: number;
  isActive: boolean;
}

export interface PromotionDto extends EntityDto<string> {
  name?: string;
  couponCode?: string;
  requireUseCouponCode: boolean;
  validDate?: string;
  expiredDate?: string;
  discountAmount: number;
  limitedUsageTimes: boolean;
  maximumDiscountAmount: number;
  isActive: boolean;
}
