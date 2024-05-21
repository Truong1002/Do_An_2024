using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ShopEcommerce.Admin.Permissions;
using ShopEcommerce.ProductCategories;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ShopEcommerce.Admin.Catalog.ProductCategories
{
    [Authorize(ShopEcommercePermissions.Category.Default, Policy = "AdminOnly")]
    public class ProductCategoriesAppService : CrudAppService<
        ProductCategory,
        ProductCategoryDto,
        Guid,
        PagedResultRequestDto,
        CreateUpdateProductCategoryDto,
        CreateUpdateProductCategoryDto>, IProductCategoriesAppService
    {
        public ProductCategoriesAppService(IRepository<ProductCategory, Guid> repository)
            : base(repository)
        {
            GetPolicyName = ShopEcommercePermissions.Category.Default;
            GetListPolicyName = ShopEcommercePermissions.Category.Default;
            CreatePolicyName = ShopEcommercePermissions.Category.Create;
            UpdatePolicyName = ShopEcommercePermissions.Category.Update;
            DeletePolicyName = ShopEcommercePermissions.Category.Delete;
        }

        [Authorize(ShopEcommercePermissions.Category.Delete)]
        public async Task DeleteMultipleAsync(IEnumerable<Guid> ids)
        {
            await Repository.DeleteManyAsync(ids);
            await UnitOfWorkManager.Current.SaveChangesAsync();
        }

        [Authorize(ShopEcommercePermissions.Category.Default)]
        public async Task<List<ProductCategoryInListDto>> GetListAllAsync()
        {
            var query = await Repository.GetQueryableAsync();
            query = query.Where(x => x.IsActive == true);
            var data = await AsyncExecuter.ToListAsync(query);

            return ObjectMapper.Map<List<ProductCategory>, List<ProductCategoryInListDto>>(data);

        }

        [Authorize(ShopEcommercePermissions.Category.Default)]
        public async Task<PagedResultDto<ProductCategoryInListDto>> GetListFilterAsync(BaseListFilterDto input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), x => x.Name.Contains(input.Keyword));

            var totalCount = await AsyncExecuter.LongCountAsync(query);
            var data = await AsyncExecuter.ToListAsync(
             query.OrderBy(x => x.SortOrder)  // Sắp xếp tăng dần theo SortOrder
             .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
    );

            return new PagedResultDto<ProductCategoryInListDto>(totalCount, ObjectMapper.Map<List<ProductCategory>, List<ProductCategoryInListDto>>(data));
        }
        [Authorize(ShopEcommercePermissions.Category.Create)]
        public async override Task<ProductCategoryDto> CreateAsync(CreateUpdateProductCategoryDto input)
        {
            await CheckDuplicateCodeAsync(input.Code);

            // Gọi phương thức Create của lớp cơ sở để tạo danh mục sản phẩm
            var productCategory = await base.CreateAsync(input);
            return productCategory;
        }

        [Authorize(ShopEcommercePermissions.Category.Update)]
        public override async Task<ProductCategoryDto> UpdateAsync(Guid id, CreateUpdateProductCategoryDto input)
        {
            await CheckDuplicateCodeAsync(input.Code, id);

            // Gọi phương thức Update của lớp cơ sở để cập nhật danh mục sản phẩm
            var productCategory = await base.UpdateAsync(id, input);
            return productCategory;
        }

        private async Task CheckDuplicateCodeAsync(string code, Guid? expectedId = null)
        {
            var existingCategory = await Repository.FirstOrDefaultAsync(c => c.Code == code);
            if (existingCategory != null && existingCategory.Id != expectedId)
            {
                throw new UserFriendlyException("Mã code sản phẩm đã tồn tại", ShopEcommerceDomainErrorCodes.CategoryCodeAlreadyExists);
            }
        }


    }
}