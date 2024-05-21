using System;
using System.Collections.Generic;
using System.Text;

namespace ShopEcommerce.Admin.Orders
{
    public class MonthlyRevenueDto
    {
        public string? Month { get; set; }
        public double TotalRevenue { get; set; }
        public double TotalDiscount { get; set; } // Thêm thuộc tính TotalDiscount
    }
}
