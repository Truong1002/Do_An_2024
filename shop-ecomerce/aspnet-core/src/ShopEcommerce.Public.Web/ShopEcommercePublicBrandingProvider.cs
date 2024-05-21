using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace ShopEcommerce.Public.Web;

[Dependency(ReplaceServices = true)]
public class ShopEcommercePublicBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Public";
}
