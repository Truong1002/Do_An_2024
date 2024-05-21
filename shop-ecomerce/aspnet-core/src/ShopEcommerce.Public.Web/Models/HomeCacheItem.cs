using System.Collections.Generic;
using ShopEcommerce.Public.ProductCategories;
using ShopEcommerce.Public.Products;

namespace ShopEcommerce.Public.Web.Models
{
    public class HomeCacheItem
    {
        public List<ProductCategoryInListDto>? Categories { set; get; }
        public List<ProductInListDto>? TopSellerProducts { set; get; }
    }
}