using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace ShopEcommerce;

[Dependency(ReplaceServices = true)]
public class ShopEcommerceBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "ShopEcommerce";
}
