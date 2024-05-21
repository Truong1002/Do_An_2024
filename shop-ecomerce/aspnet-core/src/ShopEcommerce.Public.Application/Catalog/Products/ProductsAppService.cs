using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopEcommerce.Manufacturers;
using ShopEcommerce.Orders;
using ShopEcommerce.ProductAttributes;
using ShopEcommerce.ProductCategories;
using ShopEcommerce.Products;
using ShopEcommerce.Public.Products.Attributes;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;

namespace ShopEcommerce.Public.Products
{
    public class ProductsAppService : ReadOnlyAppService<
        Product,
        ProductDto,
        Guid,
        PagedResultRequestDto>, IProductsAppService
    {
        private readonly IBlobContainer<ProductThumbnailPictureContainer> _fileContainer;
        private readonly IRepository<ProductAttribute> _productAttributeRepository;
        private readonly IRepository<ProductAttributeDateTime> _productAttributeDateTimeRepository;
        private readonly IRepository<ProductAttributeInt> _productAttributeIntRepository;
        private readonly IRepository<ProductAttributeDecimal> _productAttributeDecimalRepository;
        private readonly IRepository<ProductAttributeVarchar> _productAttributeVarcharRepository;
        private readonly IRepository<ProductAttributeText> _productAttributeTextRepository;
        private readonly IRepository<Manufacturer, Guid> _manufacturerRepository;
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IRepository<OrderItem> _orderItemRepository;
        private readonly IRepository<Order> _orderRepository;


        public ProductsAppService(IRepository<Product, Guid> repository,
            IRepository<ProductCategory> productCategoryRepository,
            IBlobContainer<ProductThumbnailPictureContainer> fileContainer,
            IRepository<ProductAttribute> productAttributeRepository,
            IRepository<ProductAttributeDateTime> productAttributeDateTimeRepository,
              IRepository<ProductAttributeInt> productAttributeIntRepository,
              IRepository<ProductAttributeDecimal> productAttributeDecimalRepository,
              IRepository<ProductAttributeVarchar> productAttributeVarcharRepository,
              IRepository<ProductAttributeText> productAttributeTextRepository,
              IRepository<Manufacturer, Guid> manufacturerRepository,
              IRepository<Product, Guid> productRepository,
              IRepository<OrderItem> orderItemRepository,
              IRepository<Order> orderRepository
              )
            : base(repository)
        {
            _fileContainer = fileContainer;
            _productAttributeRepository = productAttributeRepository;
            _productAttributeDateTimeRepository = productAttributeDateTimeRepository;
            _productAttributeIntRepository = productAttributeIntRepository;
            _productAttributeDecimalRepository = productAttributeDecimalRepository;
            _productAttributeVarcharRepository = productAttributeVarcharRepository;
            _productAttributeTextRepository = productAttributeTextRepository;
            _productRepository = productRepository;
            _manufacturerRepository = manufacturerRepository;
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
        }

        public async Task<List<ProductInListDto>> GetListAllAsync()
        {
            var query = await Repository.GetQueryableAsync();
            query = query.Where(x => x.IsActive == true);
            var data = await AsyncExecuter.ToListAsync(query);

            return ObjectMapper.Map<List<Product>, List<ProductInListDto>>(data);
        }


        public async Task<PagedResult<ProductInListDto>> GetListFilterAsync(ProductListFilterDto input)
        {
            var products = await Repository.GetQueryableAsync();  // Đảm bảo Repository này là của Product
            var manufacturers = await _manufacturerRepository.GetQueryableAsync();  // Giả sử bạn có truy cập đến Repository của Manufacturer

            // Thiết lập truy vấn ban đầu với join
            var query = from product in products
                        join manufacturer in manufacturers on product.ManufacturerId equals manufacturer.Id
                        select new
                        {
                            product,
                            manufacturer.Name,
                            manufacturer.Code
                        };

            // Áp dụng các bộ lọc
            query = query
                .WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), x => x.product.Name.Contains(input.Keyword))
                .WhereIf(input.CategoryId.HasValue, x => x.product.CategoryId == input.CategoryId)
                .WhereIf(input.ManufacturerId.HasValue, x => x.product.ManufacturerId == input.ManufacturerId);

            // Đếm tổng số bản ghi sau khi lọc nhưng trước khi phân trang
            var totalCount = await AsyncExecuter.LongCountAsync(query);        
    
            // Áp dụng phân trang
            var data = await AsyncExecuter.ToListAsync(
                query.OrderBy(x => x.product.CreationTime)
                     .Skip((input.CurrentPage - 1) * input.PageSize)
                     .Take(input.PageSize));

            // Chuyển đổi dữ liệu thô thành DTO
            var resultData = data.Select(x => new ProductInListDto
            {
                Id = x.product.Id,
                Name = x.product.Name,
                ManufacturerName = x.Name,
                Code = x.product.Code,
                Slug = x.product.Slug,
                ProductType = x.product.ProductType,
                SKU = x.product.SKU,
                ThumbnailPicture = x.product.ThumbnailPicture,
                SellPrice = x.product.SellPrice,
                CategoryName = x.product.CategoryName,
                CategorySlug = x.product.CategorySlug,
                CategoryId = x.product.CategoryId,
                ManufacturerCode=x.Code
            }).ToList();

            // Trả về kết quả cuối cùng với thông tin phân trang
            return new PagedResult<ProductInListDto>(
                resultData,
                totalCount,
                input.CurrentPage,
                input.PageSize
            );
        }


        public async Task<string> GetThumbnailImageAsync(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }
            var thumbnailContent = await _fileContainer.GetAllBytesOrNullAsync(fileName);

            if (thumbnailContent is null)
            {
                return null;
            }
            var result = Convert.ToBase64String(thumbnailContent);
            return result;
        }

        public async Task<List<ProductAttributeValueDto>> GetListProductAttributeAllAsync(Guid productId)
        {
            var attributeQuery = await _productAttributeRepository.GetQueryableAsync();

            var attributeDateTimeQuery = await _productAttributeDateTimeRepository.GetQueryableAsync();
            var attributeDecimalQuery = await _productAttributeDecimalRepository.GetQueryableAsync();
            var attributeIntQuery = await _productAttributeIntRepository.GetQueryableAsync();
            var attributeVarcharQuery = await _productAttributeVarcharRepository.GetQueryableAsync();
            var attributeTextQuery = await _productAttributeTextRepository.GetQueryableAsync();

            var query = from a in attributeQuery
                        join adate in attributeDateTimeQuery on a.Id equals adate.AttributeId into aDateTimeTable
                        from adate in aDateTimeTable.DefaultIfEmpty()
                        join adecimal in attributeDecimalQuery on a.Id equals adecimal.AttributeId into aDecimalTable
                        from adecimal in aDecimalTable.DefaultIfEmpty()
                        join aint in attributeIntQuery on a.Id equals aint.AttributeId into aIntTable
                        from aint in aIntTable.DefaultIfEmpty()
                        join aVarchar in attributeVarcharQuery on a.Id equals aVarchar.AttributeId into aVarcharTable
                        from aVarchar in aVarcharTable.DefaultIfEmpty()
                        join aText in attributeTextQuery on a.Id equals aText.AttributeId into aTextTable
                        from aText in aTextTable.DefaultIfEmpty()
                        where (adate == null || adate.ProductId == productId)
                        && (adecimal == null || adecimal.ProductId == productId)
                         && (aint == null || aint.ProductId == productId)
                          && (aVarchar == null || aVarchar.ProductId == productId)
                           && (aText == null || aText.ProductId == productId)
                        select new ProductAttributeValueDto()
                        {
                            Label = a.Label,
                            AttributeId = a.Id,
                            DataType = a.DataType,
                            Code = a.Code,
                            ProductId = productId,
                            DateTimeValue = adate != null ? adate.Value : null,
                            DecimalValue = adecimal != null ? adecimal.Value : null,
                            IntValue = aint != null ? aint.Value : null,
                            TextValue = aText != null ? aText.Value : null,
                            VarcharValue = aVarchar != null ? aVarchar.Value : null,
                            DateTimeId = adate != null ? adate.Id : null,
                            DecimalId = adecimal != null ? adecimal.Id : null,
                            IntId = aint != null ? aint.Id : null,
                            TextId = aText != null ? aText.Id : null,
                            VarcharId = aVarchar != null ? aVarchar.Id : null,
                        };
            query = query.Where(x => x.DateTimeId != null
                           || x.DecimalId != null
                           || x.IntValue != null
                           || x.TextId != null
                           || x.VarcharId != null);
            return await AsyncExecuter.ToListAsync(query);
        }


        public async Task<PagedResult<ProductAttributeValueDto>> GetListProductAttributesAsync(ProductAttributeListFilterDto input)
        {
            var attributeQuery = await _productAttributeRepository.GetQueryableAsync();

            var attributeDateTimeQuery = await _productAttributeDateTimeRepository.GetQueryableAsync();
            var attributeDecimalQuery = await _productAttributeDecimalRepository.GetQueryableAsync();
            var attributeIntQuery = await _productAttributeIntRepository.GetQueryableAsync();
            var attributeVarcharQuery = await _productAttributeVarcharRepository.GetQueryableAsync();
            var attributeTextQuery = await _productAttributeTextRepository.GetQueryableAsync();

            var query = from a in attributeQuery
                        join adate in attributeDateTimeQuery on a.Id equals adate.AttributeId into aDateTimeTable
                        from adate in aDateTimeTable.DefaultIfEmpty()
                        join adecimal in attributeDecimalQuery on a.Id equals adecimal.AttributeId into aDecimalTable
                        from adecimal in aDecimalTable.DefaultIfEmpty()
                        join aint in attributeIntQuery on a.Id equals aint.AttributeId into aIntTable
                        from aint in aIntTable.DefaultIfEmpty()
                        join aVarchar in attributeVarcharQuery on a.Id equals aVarchar.AttributeId into aVarcharTable
                        from aVarchar in aVarcharTable.DefaultIfEmpty()
                        join aText in attributeTextQuery on a.Id equals aText.AttributeId into aTextTable
                        from aText in aTextTable.DefaultIfEmpty()
                        where (adate == null || adate.ProductId == input.ProductId)
                        && (adecimal == null || adecimal.ProductId == input.ProductId)
                         && (aint == null || aint.ProductId == input.ProductId)
                          && (aVarchar == null || aVarchar.ProductId == input.ProductId)
                           && (aText == null || aText.ProductId == input.ProductId)
                        select new ProductAttributeValueDto()
                        {
                            Label = a.Label,
                            AttributeId = a.Id,
                            DataType = a.DataType,
                            Code = a.Code,
                            ProductId = input.ProductId,
                            DateTimeValue = adate != null ? adate.Value : null,
                            DecimalValue = adecimal != null ? adecimal.Value : null,
                            IntValue = aint != null ? aint.Value : null,
                            TextValue = aText != null ? aText.Value : null,
                            VarcharValue = aVarchar != null ? aVarchar.Value : null,
                            DateTimeId = adate != null ? adate.Id : null,
                            DecimalId = adecimal != null ? adecimal.Id : null,
                            IntId = aint != null ? aint.Id : null,
                            TextId = aText != null ? aText.Id : null,
                            VarcharId = aVarchar != null ? aVarchar.Id : null,
                        };
            query = query.Where(x => x.DateTimeId != null
            || x.DecimalId != null
            || x.IntValue != null
            || x.TextId != null
            || x.VarcharId != null);
            var totalCount = await AsyncExecuter.LongCountAsync(query);
            var data = await AsyncExecuter
               .ToListAsync(
                  query.Skip((input.CurrentPage - 1) * input.PageSize)
               .Take(input.PageSize));

            return new PagedResult<ProductAttributeValueDto>(data,
                totalCount,
                input.CurrentPage,
                input.PageSize
            );
        }

        /* public async Task<List<ProductInListDto>> GetListTopSellerAsync(int numberOfRecords)
         {
             var products = await _productRepository.GetQueryableAsync();
             var manufacturers = await _manufacturerRepository.GetQueryableAsync();

             // Sử dụng join để lấy thông tin Manufacturer
             var query = from product in products
                         join manufacturer in manufacturers
                         on product.ManufacturerId equals manufacturer.Id
                         where product.IsActive
                         orderby product.CreationTime descending
                         select new ProductInListDto
                         {
                             Id = product.Id,
                             Name = product.Name,
                             ManufacturerName = manufacturer.Name, // Tên nhà sản xuất
                             Code = product.Code,
                             Slug = product.Slug,
                             SKU = product.SKU,
                             ThumbnailPicture = product.ThumbnailPicture,
                             SellPrice = product.SellPrice,
                             CategoryName = product.CategoryName,
                             CategorySlug = product.CategorySlug,
                             ProductType = product.ProductType,

                         };

             var data = await AsyncExecuter.ToListAsync(query.Take(numberOfRecords));
             return data;
         }*/
        /* public async Task<List<ProductInListDto>> GetListTopSellerAsync(int numberOfRecords)
         {
             var products = await _productRepository.GetQueryableAsync();
             var manufacturers = await _manufacturerRepository.GetQueryableAsync();
             var orderItems = await _orderItemRepository.GetQueryableAsync(); // Giả định rằng bạn đã có _orderItemRepository

             // Truy vấn để lấy thông tin sản phẩm, nhà sản xuất và tổng số lượng bán
             var query = from product in products
                         join manufacturer in manufacturers
                         on product.ManufacturerId equals manufacturer.Id
                         join orderItem in orderItems
                         on product.Id equals orderItem.ProductId into productOrders
                         from order in productOrders.DefaultIfEmpty()
                         where product.IsActive
                         group new { product, manufacturer, order } by new { product.Id, product.Name, ManufacturerName = manufacturer.Name, product.Code, product.Slug, product.SKU, product.ThumbnailPicture, product.SellPrice, product.CategoryName, product.CategorySlug, product.ProductType, ManufacturerCode=manufacturer.Code } into grouped
                         orderby grouped.Sum(x => x.order != null ? x.order.Quantity : 0) descending
                         select new ProductInListDto
                         {
                             Id = grouped.Key.Id,
                             Name = grouped.Key.Name,
                             ManufacturerName = grouped.Key.ManufacturerName, // Sửa ở đây
                             Code = grouped.Key.Code,
                             Slug = grouped.Key.Slug,
                             SKU = grouped.Key.SKU,
                             ThumbnailPicture = grouped.Key.ThumbnailPicture,
                             SellPrice = grouped.Key.SellPrice,
                             CategoryName = grouped.Key.CategoryName,
                             CategorySlug = grouped.Key.CategorySlug,
                             ProductType = grouped.Key.ProductType,
                             ManufacturerCode = grouped.Key.ManufacturerCode,

                         };

             var data = await AsyncExecuter.ToListAsync(query.Take(numberOfRecords));
             return data;
         }*/
        public async Task<List<ProductInListDto>> GetListTopSellerAsync(int numberOfRecords)
        {
            var products = await _productRepository.GetQueryableAsync();
            var manufacturers = await _manufacturerRepository.GetQueryableAsync();
            var orderItems = await _orderItemRepository.GetQueryableAsync();
            var orders = await _orderRepository.GetQueryableAsync(); // Assuming _orderRepository exists and orders have a property to check if they're confirmed

            // Query to get product info, manufacturer info, and total quantity sold of confirmed orders
            var query = from product in products
                        join manufacturer in manufacturers
                        on product.ManufacturerId equals manufacturer.Id
                        join orderItem in orderItems
                        on product.Id equals orderItem.ProductId
                        join order in orders
                        on orderItem.OrderId equals order.Id
                        where product.IsActive && order.Status ==OrderStatus.Confirmed // Assuming IsConfirmed is a boolean property indicating the order's confirmation status
                        group new { product, manufacturer, orderItem } by new { product.Id, product.Name, ManufacturerName = manufacturer.Name, product.Code, product.Slug, product.SKU, product.ThumbnailPicture, product.SellPrice, product.CategoryName, product.CategorySlug, product.ProductType, ManufacturerCode = manufacturer.Code } into grouped
                        orderby grouped.Sum(x => x.orderItem.Quantity) descending
                        select new ProductInListDto
                        {
                            Id = grouped.Key.Id,
                            Name = grouped.Key.Name, 
                            ManufacturerName = grouped.Key.ManufacturerName,
                            Code = grouped.Key.Code,
                            Slug = grouped.Key.Slug,
                            SKU = grouped.Key.SKU,
                            ThumbnailPicture = grouped.Key.ThumbnailPicture,
                            SellPrice = grouped.Key.SellPrice,
                            CategoryName = grouped.Key.CategoryName,
                            CategorySlug = grouped.Key.CategorySlug,
                            ProductType = grouped.Key.ProductType,
                            ManufacturerCode = grouped.Key.ManufacturerCode,
                        };

            var data = await AsyncExecuter.ToListAsync(query.Take(numberOfRecords));
            return data;
        }


        public async Task<ProductDto> GetBySlugAsync(string slug)
        {
            var product = await _productRepository.GetAsync(x => x.Slug == slug);
            return ObjectMapper.Map<Product, ProductDto>(product);
        }
    }
}