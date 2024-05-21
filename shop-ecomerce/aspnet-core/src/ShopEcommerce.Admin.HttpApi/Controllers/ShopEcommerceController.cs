using ShopEcommerce.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ShopEcommerce.Admin.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ShopEcommerceController : AbpControllerBase
{
    protected ShopEcommerceController()
    {
        LocalizationResource = typeof(ShopEcommerceResource);
    }
}
