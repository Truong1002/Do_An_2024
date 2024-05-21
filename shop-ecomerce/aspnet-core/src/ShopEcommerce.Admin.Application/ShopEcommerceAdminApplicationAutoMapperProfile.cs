using AutoMapper;
using ShopEcommerce.ProductCategories;
using ShopEcommerce.Products;
using ShopEcommerce.Manufacturers;
using ShopEcommerce.ProductAttributes;
using ShopEcommerce.Admin.ProductAttributes;
using Volo.Abp.Identity;
using ShopEcommerce.Roles;
using ShopEcommerce.Admin.Catalog.Manufacturers;
using ShopEcommerce.Admin.Catalog.ProductCategories;
using ShopEcommerce.Admin.Catalog.Products;
using ShopEcommerce.Admin.System.Roles;
using ShopEcommerce.Admin.System.Users;
using ShopEcommerce.Orders;
using ShopEcommerce.Admin.Orders;
using ShopEcommerce.Promotions;
using ShopEcommerce.Admin.Promotions;

namespace ShopEcommerce.Admin;

public class ShopEcommerceAdminApplicationAutoMapperProfile : Profile
{
    public ShopEcommerceAdminApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        //Product Category
        CreateMap<ProductCategory, ProductCategoryDto>();
        CreateMap<ProductCategory, ProductCategoryInListDto>();
        CreateMap<CreateUpdateProductCategoryDto, ProductCategory>();

        //Product
        CreateMap<Product, ProductDto>();
        CreateMap<Product, ProductInListDto>();
        CreateMap<CreateUpdateProductDto, Product>();

        //Manufacturer
        CreateMap<Manufacturer, ManufacturerDto>();
        CreateMap<Manufacturer, ManufacturerInListDto>();
        CreateMap<CreateUpdateManufacturerDto, Manufacturer>();

        //Product attribute
        CreateMap<ProductAttribute, ProductAttributeDto>();
        CreateMap<ProductAttribute, ProductAttributeInListDto>();
        CreateMap<CreateUpdateProductAttributeDto, ProductAttribute>();

        //Roles
        CreateMap<IdentityRole, RoleDto>().ForMember(x => x.Description,
            map => map.MapFrom(x => x.ExtraProperties.ContainsKey(RoleConsts.DescriptionFieldName)
            ?
            x.ExtraProperties[RoleConsts.DescriptionFieldName]
            :
            null));
        CreateMap<IdentityRole, RoleInListDto>().ForMember(x => x.Description,
            map => map.MapFrom(x => x.ExtraProperties.ContainsKey(RoleConsts.DescriptionFieldName)
            ?
            x.ExtraProperties[RoleConsts.DescriptionFieldName]
            :
            null));
        CreateMap<CreateUpdateRoleDto, IdentityRole>();
        //User
        CreateMap<IdentityUser, UserDto>();
        CreateMap<IdentityUser, UserInListDto>();

        //Order
        CreateMap<Order, OrderDto>();
        CreateMap<OrderItem, OrderItemDto>();

        //Promotion
        CreateMap<Promotion, PromotionDto>();
        CreateMap<CreatePromotionDto, Promotion>();

    }
}