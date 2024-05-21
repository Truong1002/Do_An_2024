using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ShopEcommerce.Public.Orders
{
    public class OrderItemDto : EntityDto
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string? SKU { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        public string? ProductName { get; set; }
        public string? ProductImageUrl { get; set; }
        public string? ProductImageUrlBase64 { get; set; }  // Thêm thuộc tính nà
    }
}