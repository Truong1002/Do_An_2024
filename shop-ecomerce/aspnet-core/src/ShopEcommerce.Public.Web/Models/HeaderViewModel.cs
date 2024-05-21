using ShopEcommerce.Public.ProductCategories;
using ShopEcommerce.Public.Web.Models; // Đảm bảo rằng namespace này chính xác cho CartItem
using System.Collections.Generic;

namespace ShopEcommerce.Public.Web.ViewModels
{
    public class HeaderViewModel
    {
        public List<ProductCategoryInListDto> Categories { set; get; }
        public List<CartItem> CartItems { get; set; }
    }
}