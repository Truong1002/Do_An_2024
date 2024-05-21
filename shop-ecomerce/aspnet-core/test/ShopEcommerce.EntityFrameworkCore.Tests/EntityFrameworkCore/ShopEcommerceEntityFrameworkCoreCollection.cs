using Xunit;

namespace ShopEcommerce.EntityFrameworkCore;

[CollectionDefinition(ShopEcommerceTestConsts.CollectionDefinitionName)]
public class ShopEcommerceEntityFrameworkCoreCollection : ICollectionFixture<ShopEcommerceEntityFrameworkCoreFixture>
{

}
