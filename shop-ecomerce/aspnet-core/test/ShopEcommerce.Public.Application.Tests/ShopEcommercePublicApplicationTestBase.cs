using Volo.Abp.Modularity;

namespace ShopEcommerce.Public;

public abstract class ShopEcommercePublicApplicationTestBase<TStartupModule> : ShopEcommerceTestBase<ShopEcommercePublicApplicationTestModule>
    where TStartupModule : IAbpModule
{

}
