using Volo.Abp.Modularity;

namespace ShopEcommerce.Public;

/* Inherit from this class for your domain layer tests. */
public abstract class PublicDomainTestBase<TStartupModule> : ShopEcommerceTestBase<PublicDomainTestModule>
    where TStartupModule : IAbpModule
{

}
