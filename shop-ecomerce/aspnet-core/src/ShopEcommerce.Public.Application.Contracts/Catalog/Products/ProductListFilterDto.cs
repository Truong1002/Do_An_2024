using System;
using System.Collections.Generic;
using System.Text;

namespace ShopEcommerce.Public.Products
{
    public class ProductListFilterDto : BaseListFilterDto
    {
        public Guid? CategoryId { get; set; }
        public Guid? ManufacturerId { get; set; }
    }
}