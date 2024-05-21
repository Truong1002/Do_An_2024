using System;
using ShopEcommerce.Public.Products;

namespace ShopEcommerce.Public.Web.Models
{
    public class CartItem
    {
        public ProductDto? Product { get; set; }
        public int Quantity { get; set; }

    }
}