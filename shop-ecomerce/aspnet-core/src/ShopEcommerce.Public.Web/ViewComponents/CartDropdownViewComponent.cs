using Microsoft.AspNetCore.Mvc;
using ShopEcommerce.Public.Web.Models;
using ShopEcommerce.Public.Products;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class CartDropdownViewComponent : ViewComponent
{
    private readonly IProductsAppService _productsAppService;

    public CartDropdownViewComponent(IProductsAppService productsAppService)
    {
        _productsAppService = productsAppService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var cartJson = HttpContext.Session.GetString("Cart");
        Dictionary<string, CartItem> cartItemsDict = string.IsNullOrEmpty(cartJson)
                        ? new Dictionary<string, CartItem>()
                        : JsonSerializer.Deserialize<Dictionary<string, CartItem>>(cartJson);

        var cartItems = cartItemsDict.Values.ToList();

        foreach (var item in cartItems)
        {
            if (!string.IsNullOrEmpty(item.Product.ThumbnailPicture))
            {
                var base64Image = await _productsAppService.GetThumbnailImageAsync(item.Product.ThumbnailPicture);
                if (!string.IsNullOrEmpty(base64Image))
                {
                    // Đảm bảo rằng chuỗi base64 được định dạng đúng cho việc sử dụng trong thẻ img
                    item.Product.ThumbnailPictureBase64 = $"data:image/jpeg;base64,{base64Image}";
                }
            }
        }

        return View(cartItems);
    }
}