using ShopEcommerce.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ShopEcommerce.Admin.Permissions;

public class ShopEcommercePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        //Catalog
        var catalogGroup = context.AddGroup(ShopEcommercePermissions.CatalogGroupName, L("Permission:Catalog"));
        var orderGroup = context.AddGroup(ShopEcommercePermissions.OrderGroupName, L("Permission:Order"));
        //Manufacture
        var manufacturerPermission = catalogGroup.AddPermission(ShopEcommercePermissions.Manufacturer.Default, L("Permission:Catalog.Manufacturer"));
        manufacturerPermission.AddChild(ShopEcommercePermissions.Manufacturer.Create, L("Permission:Catalog.Manufacturer.Create"));
        manufacturerPermission.AddChild(ShopEcommercePermissions.Manufacturer.Update, L("Permission:Catalog.Manufacturer.Update"));
        manufacturerPermission.AddChild(ShopEcommercePermissions.Manufacturer.Delete, L("Permission:Catalog.Manufacturer.Delete"));

        //Add product
        var productPermission = catalogGroup.AddPermission(ShopEcommercePermissions.Product.Default, L("Permission:Catalog.Product"));
        productPermission.AddChild(ShopEcommercePermissions.Product.Create, L("Permission:Catalog.Product.Create"));
        productPermission.AddChild(ShopEcommercePermissions.Product.Update, L("Permission:Catalog.Product.Update"));
        productPermission.AddChild(ShopEcommercePermissions.Product.Delete, L("Permission:Catalog.Product.Delete"));
        productPermission.AddChild(ShopEcommercePermissions.Product.AttributeManage, L("Permission:Catalog.Product.AttributeManage"));

        //Add attribute
        var attributePermission = catalogGroup.AddPermission(ShopEcommercePermissions.Attribute.Default, L("Permission:Catalog.Attribute"));
        attributePermission.AddChild(ShopEcommercePermissions.Attribute.Create, L("Permission:Catalog.Attribute.Create"));
        attributePermission.AddChild(ShopEcommercePermissions.Attribute.Update, L("Permission:Catalog.Attribute.Update"));
        attributePermission.AddChild(ShopEcommercePermissions.Attribute.Delete, L("Permission:Catalog.Attribute.Delete"));
        //Add category
        var categoryPermission = catalogGroup.AddPermission(ShopEcommercePermissions.Category.Default, L("Permission:Catalog.Category"));
        categoryPermission.AddChild(ShopEcommercePermissions.Category.Create, L("Permission:Catalog.Category.Create"));
        categoryPermission.AddChild(ShopEcommercePermissions.Category.Update, L("Permission:Catalog.Category.Update"));
        categoryPermission.AddChild(ShopEcommercePermissions.Category.Delete, L("Permission:Catalog.Category.Delete"));

        //Add Order
        var orderPermission = orderGroup.AddPermission(ShopEcommercePermissions.Order.Default, L("Permission:Order"));
        orderPermission.AddChild(ShopEcommercePermissions.Order.Create, L("Permission:Order.Create"));
        orderPermission.AddChild(ShopEcommercePermissions.Order.Update, L("Permission:Order.Update"));
        orderPermission.AddChild(ShopEcommercePermissions.Order.Delete, L("Permission:Order.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ShopEcommerceResource>(name);
    }
}