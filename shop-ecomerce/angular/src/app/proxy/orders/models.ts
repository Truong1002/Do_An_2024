import type { AuditedEntityDto, EntityDto } from '@abp/ng.core';
import type { OrderStatus } from '../shop-ecommerce/orders/order-status.enum';
import type { PaymentMethod } from '../shop-ecommerce/orders/payment-method.enum';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

export interface CreateOrderDto {
  customerName?: string;
  customerPhoneNumber?: string;
  customerAddress?: string;
  customerUserId?: string;
  items: OrderItemDto[];
}

export interface MonthlyRevenueDto {
  month?: string;
  totalRevenue: number;
}

export interface OrderDto extends AuditedEntityDto<string> {
  code?: string;
  status: OrderStatus;
  paymentMethod: PaymentMethod;
  shippingFee: number;
  tax: number;
  total: number;
  subtotal: number;
  discount: number;
  grandTotal: number;
  customerName?: string;
  customerPhoneNumber?: string;
  customerAddress?: string;
  customerUserId?: string;
}

export interface OrderItemDto extends EntityDto {
  orderId?: string;
  productId?: string;
  sku?: string;
  quantity: number;
  price: number;
}

export interface ProductSalesDto extends EntityDto<string> {
  productId?: string;
  productName?: string;
  quantitySold: number;
  totalRevenue: number;
  manufacturerName?: string;
  thumbnailPicture?: string;
  saleDate?: string;
  safeThumbnailUrl?:SafeUrl;
}

export interface ProductSalesTimeDto {
  productId?: string;
  productName?: string;
  quantitySold: number;
  totalRevenue: number;
  total: number;
  manufacturerName?: string;
  thumbnailPicture?: string;
  discount: number;
  saleDate?: string;
  safeThumbnailUrl?:SafeUrl;
}
