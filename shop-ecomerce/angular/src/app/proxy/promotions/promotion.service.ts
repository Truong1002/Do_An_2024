import type { CreatePromotionDto, PromotionDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto, PagedResultRequestDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { BaseListFilterDto } from '../models';

@Injectable({
  providedIn: 'root',
})
export class PromotionService {
  apiName = 'Default';
  

  activatePromotion = (promotionId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/promotion/activate-promotion/${promotionId}`,
    },
    { apiName: this.apiName,...config });
  

  create = (input: CreatePromotionDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PromotionDto>({
      method: 'POST',
      url: '/api/app/promotion',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  deactivatePromotion = (promotionId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/promotion/deactivate-promotion/${promotionId}`,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/promotion/${id}`,
    },
    { apiName: this.apiName,...config });
  

  deleteMultiple = (ids: string[], config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: '/api/app/promotion/multiple',
      params: { ids },
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PromotionDto>({
      method: 'GET',
      url: `/api/app/promotion/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getAllPromotions = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, PromotionDto[]>({
      method: 'GET',
      url: '/api/app/promotion/promotions',
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PromotionDto>>({
      method: 'GET',
      url: '/api/app/promotion',
      params: { skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getListFilter = (input: BaseListFilterDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PromotionDto>>({
      method: 'GET',
      url: '/api/app/promotion/filter',
      params: { keyword: input.keyword, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreatePromotionDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PromotionDto>({
      method: 'PUT',
      url: `/api/app/promotion/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  validateCouponCode = (couponCode: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, boolean>({
      method: 'POST',
      url: '/api/app/promotion/validate-coupon-code',
      params: { couponCode },
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
