using System;
using System.Collections.Generic;
using System.Text;

namespace ShopEcommerce.Admin.Orders
{
    public class ProductSalesTimeDto
    {
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public int QuantitySold { get; set; }
        public double TotalRevenue { get; set; }
        public double Total {  get; set; }
        public string? ManufacturerName { get; set; }
        public string? ThumbnailPicture { get; set; }
        public double Discount {  get; set; }
        public DateTime SaleDate { get; set; }
    }
}
