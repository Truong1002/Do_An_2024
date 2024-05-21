using Volo.Abp.Modularity;

namespace ShopEcommerce.Public;

[DependsOn(
    typeof(ShopEcommerceDomainModule),
    typeof(ShopEcommerceTestBaseModule)
)]
public class PublicDomainTestModule : AbpModule
{

}
