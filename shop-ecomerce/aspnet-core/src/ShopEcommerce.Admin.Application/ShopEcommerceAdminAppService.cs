using System;
using System.Collections.Generic;
using System.Text;
using ShopEcommerce.Localization;
using Volo.Abp.Application.Services;

namespace ShopEcommerce.Admin;

/* Inherit your application services from this class.
 */
public abstract class ShopEcommerceAdminAppService : ApplicationService
{
    protected ShopEcommerceAdminAppService()
    {
        LocalizationResource = typeof(ShopEcommerceResource);
    }
}
