using ShopEcommerce.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ShopEcommerce.Public.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ShopEcommercePublicController : AbpControllerBase
{
    protected ShopEcommercePublicController()
    {
        LocalizationResource = typeof(ShopEcommerceResource);
    }
}
