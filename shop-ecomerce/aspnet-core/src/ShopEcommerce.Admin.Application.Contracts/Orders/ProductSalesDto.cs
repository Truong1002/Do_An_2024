using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ShopEcommerce.Admin.Orders
{
    public class ProductSalesDto : EntityDto<Guid>
    {
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public int QuantitySold { get; set; }
        public double TotalRevenue { get; set; }
        public string? ManufacturerName {  get; set; }
        public string? ThumbnailPicture { get; set; }
        public DateTime SaleDate { get; set; }
    }
}
