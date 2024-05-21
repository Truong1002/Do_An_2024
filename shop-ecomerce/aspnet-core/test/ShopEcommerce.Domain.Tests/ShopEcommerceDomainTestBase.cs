using Volo.Abp.Modularity;

namespace ShopEcommerce;

/* Inherit from this class for your domain layer tests. */
public abstract class ShopEcommerceDomainTestBase<TStartupModule> : ShopEcommerceTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
