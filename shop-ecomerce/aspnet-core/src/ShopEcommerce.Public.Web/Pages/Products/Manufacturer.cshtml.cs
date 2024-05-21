using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopEcommerce.Public.Manufacturers;
using System.Collections.Generic;
using ShopEcommerce.Public.Products;
using System.Threading.Tasks;
using System;

namespace ShopEcommerce.Public.Web.Pages.Products
{
    [Route("/manufacturer/{code}")]
    public class ManufacturerModel : PageModel
    {
        public ManufacturerDto Manufacturer { set; get; }

        public List<ManufacturerInListDto> Manufacturers{ set; get; }
        public PagedResult<ProductInListDto> ProductData { set; get; }

        private readonly IManufacturersAppService _manufacturersAppService;
        private readonly IProductsAppService _productsAppService;

        public ManufacturerModel(IManufacturersAppService manufacturersAppService,
            IProductsAppService productsAppService)
        {
            _manufacturersAppService = manufacturersAppService;
            _productsAppService = productsAppService;
        }

        public async Task OnGetAsync([FromRoute] string code, [FromQuery] int page = 1)
        {
            Manufacturer= await _manufacturersAppService.GetByCodeAsync(code);
            if (Manufacturer != null)
            {
                Manufacturers = await _manufacturersAppService.GetListAllAsync();

                // Thiết lập bộ lọc với CategoryId để lấy sản phẩm chỉ trong danh mục này
                ProductData = await _productsAppService.GetListFilterAsync(new ProductListFilterDto()
                {
                    ManufacturerId = Manufacturer.Id, // Sử dụng Id của danh mục được lấy bởi code
                    CurrentPage = page
                }); ;

            }

        }
    }
}
