using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ShopEcommerce.Public.ProductCategories;
using ShopEcommerce.Public.Web.Models;
using Volo.Abp.Caching;
using ShopEcommerce.Public.Manufacturers;

namespace ShopEcommerce.Public.Web.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly IProductCategoriesAppService _productCategoriesAppService;
        private readonly IDistributedCache<HeaderCacheItem> _distributedCache;
        private readonly IManufacturersAppService _manufacturersAppService;
        public HeaderViewComponent(IProductCategoriesAppService productCategoriesAppService,
            IDistributedCache<HeaderCacheItem> distributedCache,
            IManufacturersAppService manufacturersAppService)
        {
            _productCategoriesAppService = productCategoriesAppService;
            _distributedCache = distributedCache;
            _manufacturersAppService = manufacturersAppService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cacheItem = await _distributedCache.GetOrAddAsync(
                ShopEcommercePublicConsts.CacheKeys.HeaderData, async () =>
                {
                    var categories = await _productCategoriesAppService.GetListAllAsync();
                    var manufacturers = await _manufacturersAppService.GetListAllAsync();
                    return new HeaderCacheItem()
                    {
                        Categories = categories,
                        Manufacturers = manufacturers // Lưu trữ dữ liệu nhà sản xuất vào cache item
                    };
                },
            () => new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddHours(12)
            });
            return View(cacheItem);
        }
    }
}