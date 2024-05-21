using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using ShopEcommerce.Public.ProductCategories;
using ShopEcommerce.Public.Products;
using ShopEcommerce.Public.Products.Attributes;
using System.Collections.Generic;
using ShopEcommerce.Public.Web.Models;
using ShopEcommerce.Public.Manufacturers;

namespace ShopEcommerce.Public.Web.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly IProductsAppService _productsAppService;
        private readonly IProductCategoriesAppService _productCategoriesAppService;
        private readonly IManufacturersAppService _manufacturerService;
        public DetailsModel(IProductsAppService productsAppService,
            IProductCategoriesAppService productCategoriesAppService,
            IManufacturersAppService manufacturerAppService)
        {
            _productsAppService = productsAppService;
            _productCategoriesAppService = productCategoriesAppService;
            _manufacturerService = manufacturerAppService;
        }
        
         public ProductCategoryDto Category { get; set; }
        public ProductDto Product { get; set; }

   
        public List<ProductAttributeValueDto> ProductAttributes { get; set; } // Thêm thuộc tính này

        public string ManufacturerName { get; set; }
        public string ManufacturerCode { get; set; }


        public async Task OnGetAsync(string categorySlug, string slug)
        {
            Category = await _productCategoriesAppService.GetBySlugAsync(categorySlug);
            Product = await _productsAppService.GetBySlugAsync(slug);
            ProductAttributes = await _productsAppService.GetListProductAttributeAllAsync(Product.Id);
            var manufacturer = await _manufacturerService.GetManufacturerByIdAsync(Product.ManufacturerId);
            ManufacturerName = manufacturer.Name;
            ManufacturerCode = manufacturer.Code;
        }
    }
}