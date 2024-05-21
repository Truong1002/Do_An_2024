import type { CreateOrderDto, MonthlyRevenueDto, OrderDto, OrderItemDto, ProductSalesDto, ProductSalesTimeDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto, PagedResultRequestDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { BaseListFilterDto } from '../models';

@Injectable({
  providedIn: 'root',
})
export class OrdersService {
  apiName = 'Default';
  

  confirmOrder = (orderId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/orders/confirm-order/${orderId}`,
    },
    { apiName: this.apiName,...config });
  

  create = (input: CreateOrderDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, OrderDto>({
      method: 'POST',
      url: '/api/app/orders',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/orders/${id}`,
    },
    { apiName: this.apiName,...config });
  

  deleteMultiple = (ids: string[], config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: '/api/app/orders/multiple',
      params: { ids },
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, OrderDto>({
      method: 'GET',
      url: `/api/app/orders/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getAllOrders = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, OrderDto[]>({
      method: 'GET',
      url: '/api/app/orders/orders',
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<OrderDto>>({
      method: 'GET',
      url: '/api/app/orders',
      params: { skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getListFilter = (input: BaseListFilterDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<OrderDto>>({
      method: 'GET',
      url: '/api/app/orders/filter',
      params: { keyword: input.keyword, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getMonthlyRevenueStatisticsForYear = (year: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, MonthlyRevenueDto[]>({
      method: 'GET',
      url: '/api/app/orders/monthly-revenue-statistics-for-year',
      params: { year },
    },
    { apiName: this.apiName,...config });
  

  getOrderItems = (orderId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, OrderItemDto[]>({
      method: 'GET',
      url: `/api/app/orders/order-items/${orderId}`,
    },
    { apiName: this.apiName,...config });
  

  getProductSalesStatistics = (input: BaseListFilterDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ProductSalesDto>>({
      method: 'GET',
      url: '/api/app/orders/product-sales-statistics',
      params: { keyword: input.keyword, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getProductSalesStatisticsByTime = (input: BaseListFilterDto, startDate: string, endDate: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ProductSalesTimeDto>>({
      method: 'GET',
      url: '/api/app/orders/product-sales-statistics-by-time',
      params: { keyword: input.keyword, skipCount: input.skipCount, maxResultCount: input.maxResultCount, startDate, endDate },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateOrderDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, OrderDto>({
      method: 'PUT',
      url: `/api/app/orders/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
