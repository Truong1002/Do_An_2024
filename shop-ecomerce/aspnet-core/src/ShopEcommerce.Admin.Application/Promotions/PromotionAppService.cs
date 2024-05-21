using AutoMapper.Internal.Mappers;
using ShopEcommerce.Promotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ShopEcommerce.Admin.Promotions
{
    public class PromotionAppService : CrudAppService<
        Promotion,
        PromotionDto,
        Guid,
        PagedResultRequestDto,
        CreatePromotionDto,
        CreatePromotionDto>, IPromotionAppService
    {
        public PromotionAppService(IRepository<Promotion, Guid> repository) : base(repository)
        {
            // Initialize policy names if applicable
            // Example:
            // GetPolicyName = ShopEcommercePermissions.Promotions.Default;
        }

        public async Task<List<PromotionDto>> GetAllPromotionsAsync()
        {
            var query = await Repository.GetQueryableAsync();
            var promotions = await AsyncExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<Promotion>, List<PromotionDto>>(promotions);
        }

        public async Task<PagedResultDto<PromotionDto>> GetListFilterAsync(BaseListFilterDto input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), x => x.Name.Contains(input.Keyword) || x.CouponCode.Contains(input.Keyword));

            var totalCount = await AsyncExecuter.LongCountAsync(query);
            var promotions = await AsyncExecuter.ToListAsync(
                query.Skip(input.SkipCount).Take(input.MaxResultCount)
            );

            return new PagedResultDto<PromotionDto>(totalCount, ObjectMapper.Map<List<Promotion>, List<PromotionDto>>(promotions));
        }

        public async Task ActivatePromotionAsync(Guid promotionId)
        {
            var promotion = await Repository.GetAsync(promotionId);
            promotion.IsActive = true;
            await Repository.UpdateAsync(promotion);
        }

        public async Task DeactivatePromotionAsync(Guid promotionId)
        {
            var promotion = await Repository.GetAsync(promotionId);
            promotion.IsActive = false;
            await Repository.UpdateAsync(promotion);
        }

        public async Task DeleteMultipleAsync(IEnumerable<Guid> ids)
        {
            await Repository.DeleteManyAsync(ids);
        }

        public async Task<bool> ValidateCouponCodeAsync(string couponCode)
        {
            var existingPromotion = await Repository.FirstOrDefaultAsync(p => p.CouponCode == couponCode && p.IsActive);
            return existingPromotion != null;
        }

        public async override Task<PromotionDto> CreateAsync(CreatePromotionDto input)
        {
            // Validate for duplicate Coupon Code
            await CheckDuplicateCouponCodeAsync(input.CouponCode);

            // Map DTO to Entity and Save to Database
            var promotion = ObjectMapper.Map<CreatePromotionDto, Promotion>(input);
            var createdPromotion = await Repository.InsertAsync(promotion);
            return ObjectMapper.Map<Promotion, PromotionDto>(createdPromotion);
        }

        // Update an existing promotion
        public async override Task<PromotionDto> UpdateAsync(Guid id, CreatePromotionDto input)
        {
            // Check for duplicate Coupon Code, ignoring the current promotion
            await CheckDuplicateCouponCodeAsync(input.CouponCode, id);

            // Fetch and update the existing promotion
            var promotion = await Repository.GetAsync(id);
            ObjectMapper.Map(input, promotion);
            var updatedPromotion = await Repository.UpdateAsync(promotion);
            return ObjectMapper.Map<Promotion, PromotionDto>(updatedPromotion);
        }

        // Check for duplicate Coupon Code
        private async Task CheckDuplicateCouponCodeAsync(string couponCode, Guid? expectedId = null)
        {
            var existingPromotion = await Repository.FirstOrDefaultAsync(p => p.CouponCode == couponCode);
            if (existingPromotion != null && existingPromotion.Id != expectedId)
            {
                throw new UserFriendlyException("Coupon code already exists.", ShopEcommerceDomainErrorCodes.PromotionCouponCodeAlreadyExists);
            }
        }
    }
}
