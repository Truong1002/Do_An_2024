using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopEcommerce.Public.Products;  // Make sure this using directive is correct
using System;
using System.Threading.Tasks;

namespace ShopEcommerce.Public.Web.Pages.Products
{
    public class SearchModel : PageModel
    {
        private readonly IProductsAppService _productAppService;
        public PagedResult<ProductInListDto> Products { get; set; }

        public SearchModel(IProductsAppService productAppService)
        {
            _productAppService = productAppService;
        }

        public async Task OnGetAsync([FromQuery] string keyword, [FromQuery] Guid? categoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 8)
        {
            var input = new ProductListFilterDto
            {
                Keyword = keyword,
                CategoryId = categoryId,
                CurrentPage = page,
                PageSize = pageSize
            };

            // Gọi dịch vụ để lấy danh sách sản phẩm với bộ lọc
            Products = await _productAppService.GetListFilterAsync(input);
        }
    }
}