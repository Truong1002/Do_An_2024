using Volo.Abp.Modularity;

namespace ShopEcommerce.Admin;

[DependsOn(
    typeof(ShopEcommerceAdminApplicationModule),
    typeof(ShopEcommerceDomainTestModule)
)]
public class ShopEcommerceApplicationTestModule : AbpModule
{

}
