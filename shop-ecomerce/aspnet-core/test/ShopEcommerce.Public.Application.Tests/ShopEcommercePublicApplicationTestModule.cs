using Volo.Abp.Modularity;

namespace ShopEcommerce.Public;

[DependsOn(
    typeof(ShopEcommercePublicApplicationModule),
    typeof(ShopEcommerceDomainTestModule)
)]
public class ShopEcommercePublicApplicationTestModule : AbpModule
{

}
