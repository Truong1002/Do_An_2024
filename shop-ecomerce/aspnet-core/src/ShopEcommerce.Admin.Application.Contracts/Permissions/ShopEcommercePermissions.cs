namespace ShopEcommerce.Admin.Permissions;

public static class ShopEcommercePermissions
{
    public const string SystemGroupName = "ShopEcomAdminSystem";
    public const string CatalogGroupName = "ShopEcomAdminCatalog";
    public const string OrderGroupName = "ShopEcomAdminOrder";

    //Add your own permission names. Example:


    public static class Product
    {
        public const string Default = CatalogGroupName + ".Product";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string AttributeManage = Default + ".Attribute";

    }

    public static class Attribute
    {
        public const string Default = CatalogGroupName + ".Attribute";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class Category
    {
        public const string Default = CatalogGroupName + ".Category";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class Manufacturer
    {
        public const string Default = CatalogGroupName + ".Manufacturer";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class Order
    {
        public const string Default = OrderGroupName + ".Order";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

}