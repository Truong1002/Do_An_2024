using System;
using System.Collections.Generic;
using System.Text;
using ShopEcommerce.Localization;
using Volo.Abp.Application.Services;

namespace ShopEcommerce.Public;

/* Inherit your application services from this class.
 */
public abstract class ShopEcommercePublicAppService : ApplicationService
{
    protected ShopEcommercePublicAppService()
    {
        LocalizationResource = typeof(ShopEcommerceResource);
    }
}
