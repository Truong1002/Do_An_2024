using System.Threading.Tasks;

namespace ShopEcommerce.Data;

public interface IShopEcommerceDbSchemaMigrator
{
    Task MigrateAsync();
}
