
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShopEcommerce.Public.ProductCategories;
using ShopEcommerce.Public.Products;
using Microsoft.AspNetCore.Mvc;

namespace ShopEcommerce.Public.Web.Pages.Products
{
    public class CategoryModel : PageModel
    {
        public ProductCategoryDto Category { set; get; }

        public List<ProductCategoryInListDto> Categories { set; get; }
        public PagedResult<ProductInListDto> ProductData { set; get; }

        private readonly IProductCategoriesAppService _productCategoriesAppService;
        private readonly IProductsAppService _productsAppService;

        public CategoryModel(IProductCategoriesAppService productCategoriesAppService,
            IProductsAppService productsAppService)
        {
            _productCategoriesAppService = productCategoriesAppService;
            _productsAppService = productsAppService;
        }

        public async Task OnGetAsync([FromRoute]string code, [FromQuery] int page = 1)
        {          
            Category = await _productCategoriesAppService.GetByCodeAsync(code);
            if (Category != null)
            {
                Categories = await _productCategoriesAppService.GetListAllAsync();

                // Thiết lập bộ lọc với CategoryId để lấy sản phẩm chỉ trong danh mục này
                ProductData = await _productsAppService.GetListFilterAsync(new ProductListFilterDto()
                {
                    CategoryId = Category.Id, // Sử dụng Id của danh mục được lấy bởi code
                    CurrentPage = page
                }); ;
                
            }
            
        }
    }
}