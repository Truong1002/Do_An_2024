using System.Collections.Generic;
using ShopEcommerce.Public.ProductCategories;
using ShopEcommerce.Public.Manufacturers;

namespace ShopEcommerce.Public.Web.Models
{
    public class HeaderCacheItem
    {
        public List<ProductCategoryInListDto> Categories { set; get; }
        public List<ManufacturerInListDto> Manufacturers { set; get; }
    }
}