using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ShopEcommerce.Public.Products;
using ShopEcommerce.Public.Web.Models;

namespace ShopEcommerce.Public.Web.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly IProductsAppService _productsAppService;

        public IndexModel(IProductsAppService productsAppService)
        {
            _productsAppService = productsAppService;
        }

        [BindProperty]
        public List<CartItem> CartItems { get; set; }

        private void UpdateSessionCart(Dictionary<string, CartItem> productCarts)
        {
            HttpContext.Session.SetString(ShopEcommerceConsts.Cart, JsonSerializer.Serialize(productCarts));
        }

        private Dictionary<string, CartItem> GetCart()
        {
            var cart = HttpContext.Session.GetString(ShopEcommerceConsts.Cart);
            return string.IsNullOrEmpty(cart) ? new Dictionary<string, CartItem>() : JsonSerializer.Deserialize<Dictionary<string, CartItem>>(cart);
        }

        public async Task OnGetAsync(string action, string id)
        {
            var productCarts = GetCart();
            if (!string.IsNullOrEmpty(action))
            {
                if (action == "add" && Guid.TryParse(id, out Guid productId))
                {
                    var product = await _productsAppService.GetAsync(productId);
                    if (product != null)
                    {
                        var base64Image = await _productsAppService.GetThumbnailImageAsync(product.ThumbnailPicture);
                        if (!string.IsNullOrEmpty(base64Image))
                        {
                            product.ThumbnailPictureBase64 = $"data:image/jpeg;base64,{base64Image}";
                        }

                        if (productCarts.ContainsKey(id))
                        {
                            productCarts[id].Quantity += 1;
                        }
                        else
                        {
                            productCarts.Add(id, new CartItem { Product = product, Quantity = 1 });
                        }
                    }
                }
                else if (action == "remove" && productCarts.ContainsKey(id))
                {
                    productCarts.Remove(id);
                }
                UpdateSessionCart(productCarts);
            }
            CartItems = productCarts.Values.ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var productCarts = GetCart();
            foreach (var item in CartItems)
            {
                if (productCarts.TryGetValue(item.Product.Id.ToString(), out CartItem storedItem))
                {
                    storedItem.Quantity = item.Quantity;
                    storedItem.Product = await _productsAppService.GetAsync(storedItem.Product.Id);
                    var base64Image = await _productsAppService.GetThumbnailImageAsync(storedItem.Product.ThumbnailPicture);
                    if (!string.IsNullOrEmpty(base64Image))
                    {
                        storedItem.Product.ThumbnailPictureBase64 = $"data:image/jpeg;base64,{base64Image}";
                    }
                }
            }
            UpdateSessionCart(productCarts);
            return Redirect("/shop-cart.html");
        }
    }
}