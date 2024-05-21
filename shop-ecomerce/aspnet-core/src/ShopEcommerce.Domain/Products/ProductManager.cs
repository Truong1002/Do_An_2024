using ShopEcommerce.ProductCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace ShopEcommerce.Products
{
    public class ProductManager:DomainService
    {
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IRepository<ProductCategory, Guid> _productCategoryRepository;

        public ProductManager(IRepository<Product, Guid> productRepository, 
            IRepository<ProductCategory, Guid> productCategoryRepository)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
        }

        public async Task<Product> CreateAsync(Guid manufacturerId, string? name, string? code, string? slug,
            ProductType productType, string? sKU, int sortOrder,
            bool visibility, bool isActive, Guid categoryId,
            string? seoMetaDescription, string? description,
            int stockQuantity, double sellPrice)
        {
            if (await _productRepository.AnyAsync(x => x.Code == code))
                throw new UserFriendlyException("Mã sản phẩm đã tồn tại", ShopEcommerceDomainErrorCodes.ProductCodeAlreadyExists);
            if (await _productRepository.AnyAsync(x => x.SKU == sKU))
                throw new UserFriendlyException("Mã SKU sản phẩm đã tồn tại", ShopEcommerceDomainErrorCodes.ProductSKUAlreadyExists);

            var category =await _productCategoryRepository.GetAsync(categoryId);
            return new Product(Guid.NewGuid(), manufacturerId, name, code, slug, productType, sKU, sortOrder,
               visibility, isActive, categoryId, seoMetaDescription, description,stockQuantity,null, sellPrice, category?.Name, category?.Slug);
        }

        public async Task<Product> UpdateAsync(Guid manufacturerId, string? name, string? code, string? slug,
           ProductType productType, string? sKU, int sortOrder,
           bool visibility, bool isActive, Guid categoryId,
           string? seoMetaDescription, string? description,
           int stockQuantity, double sellPrice)
        {
            if (await _productRepository.AnyAsync(x => x.Code == code))
                throw new UserFriendlyException("Mã sản phẩm đã tồn tại", ShopEcommerceDomainErrorCodes.ProductCodeAlreadyExists);
            if (await _productRepository.AnyAsync(x => x.SKU == sKU))
                throw new UserFriendlyException("Mã SKU sản phẩm đã tồn tại", ShopEcommerceDomainErrorCodes.ProductSKUAlreadyExists);

            var category = await _productCategoryRepository.GetAsync(categoryId);
            return new Product(Guid.NewGuid(), manufacturerId, name, code, slug, productType, sKU, sortOrder,
               visibility, isActive, categoryId, seoMetaDescription, description, stockQuantity, null, sellPrice, category?.Name, category?.Slug);
        }
    }
}
