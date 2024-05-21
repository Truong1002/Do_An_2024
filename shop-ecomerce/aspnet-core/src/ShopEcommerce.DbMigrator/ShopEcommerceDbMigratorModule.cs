using ShopEcommerce.Admin;
using ShopEcommerce.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Modularity;

namespace ShopEcommerce.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(ShopEcommerceEntityFrameworkCoreModule),
    typeof(ShopEcommerceApplicationContractsModule)
    )]
public class ShopEcommerceDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDistributedCacheOptions>(options => { options.KeyPrefix = "ShopEcommerce:"; });
    }
}
