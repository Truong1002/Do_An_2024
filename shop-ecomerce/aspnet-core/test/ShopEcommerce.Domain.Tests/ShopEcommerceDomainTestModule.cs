using Volo.Abp.Modularity;

namespace ShopEcommerce;

[DependsOn(
    typeof(ShopEcommerceDomainModule),
    typeof(ShopEcommerceTestBaseModule)
)]
public class ShopEcommerceDomainTestModule : AbpModule
{

}
