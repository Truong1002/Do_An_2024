using AutoMapper;
using ShopEcommerce.Manufacturers;
using ShopEcommerce.Orders;
using ShopEcommerce.ProductAttributes;
using ShopEcommerce.ProductCategories;
using ShopEcommerce.Products;
using ShopEcommerce.Promotions;
using ShopEcommerce.Public.Manufacturers;
using ShopEcommerce.Public.Orders;
using ShopEcommerce.Public.ProductAttributes;
using ShopEcommerce.Public.ProductCategories;
using ShopEcommerce.Public.Products;
using ShopEcommerce.Public.Promotions;

namespace ShopEcommerce.Public;

public class ShopEcommercePublicApplicationAutoMapperProfile : Profile
{
    public ShopEcommercePublicApplicationAutoMapperProfile()
    {
        //Product Category
        CreateMap<ProductCategory, ProductCategoryDto>();
        CreateMap<ProductCategory, ProductCategoryInListDto>();

        //Product
        CreateMap<Product, ProductDto>();
        CreateMap<Product, ProductInListDto>();

        CreateMap<Manufacturer, ManufacturerDto>();
        CreateMap<Manufacturer, ManufacturerInListDto>();

        //Product attribute
        CreateMap<ProductAttribute, ProductAttributeDto>();
        CreateMap<ProductAttribute, ProductAttributeInListDto>();

        //Order
        CreateMap<Order, OrderDto>();
        //Promotion
        CreateMap<Promotion, PromotionDto>();
        CreateMap<CreatePromotionDto, Promotion>();
    }
}
