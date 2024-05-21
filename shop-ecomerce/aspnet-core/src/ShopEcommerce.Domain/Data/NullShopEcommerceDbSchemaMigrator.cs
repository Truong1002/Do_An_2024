using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ShopEcommerce.Data;

/* This is used if database provider does't define
 * IShopEcommerceDbSchemaMigrator implementation.
 */
public class NullShopEcommerceDbSchemaMigrator : IShopEcommerceDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
