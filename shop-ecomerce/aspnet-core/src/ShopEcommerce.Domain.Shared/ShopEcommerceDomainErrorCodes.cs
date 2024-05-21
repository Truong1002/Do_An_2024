namespace ShopEcommerce;

public static class ShopEcommerceDomainErrorCodes
{
    /* You can add your business exception error codes here, as constants */
    public const string ProductNameAlreadyExists = "ShopEcommerce:ProductNameAlreadyExists";
    public const string ProductCodeAlreadyExists = "ShopEcommerce:ProductCodeAlreadyExists";

    public const string ProductSKUAlreadyExists = "ShopEcommerce:ProductSKUAlreadyExists";
    public const string ProductIsNotExists = "ShopEcommerce:ProductIsNotExists";
    public const string ProductAttributeIdIsNotExists = "ShopEcommerce:ProductAttributeIdIsNotExists";

    public const string ProductAttributeValueIsNotValid = "ShopEcommerce:ProductAttributeValueIsNotValid";

    public const string RoleNameAlreadyExists = "ShopEcommerce:RoleNameAlreadyExists";
    public const string CategoryCodeAlreadyExists = "ShopEcommerce:CategoryCodeAlreadyExists";
    public const string AccountAlreadyExists = "ShopEcommerce:AccountAlreadyExists";
    public const string EmailAlreadyExists = "ShopEcommerce:EmailAlreadyExists";
    public const string PromotionCouponCodeAlreadyExists = "ShopEcommerce: PromotionCodeAlreadyExist";
}