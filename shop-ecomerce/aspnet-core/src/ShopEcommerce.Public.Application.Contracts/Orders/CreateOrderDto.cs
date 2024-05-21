using ShopEcommerce.Public.Orders;
using System;
using System.Collections.Generic;
using System.Text;
using ShopEcommerce.Orders;

namespace ShopEcommerce.Public.Orders
{
    public class CreateOrderDto
    {
        public string? CustomerName { get; set; }
        public string? CustomerPhoneNumber { get; set; }
        public string? CustomerAddress { get; set; }
        public Guid? CustomerUserId { get; set; }
        public string? CouponCode { get; set; }
        public double Discount { get; set; }

        public List<OrderItemDto>? Items { get; set; }
    }
}