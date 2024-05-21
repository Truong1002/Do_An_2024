using AutoMapper.Internal.Mappers;
using ShopEcommerce.Promotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp;

namespace ShopEcommerce.Public.Promotions
{
    public class PromotionAppService : CrudAppService<
       Promotion,
       PromotionDto,
       Guid,
       PagedResultRequestDto,
       CreatePromotionDto,
       CreatePromotionDto>, IPromotionAppService
    {

        private readonly IRepository<Promotion, Guid> _promotionRepository;
        public PromotionAppService(IRepository<Promotion, Guid> repository) : base(repository)
        {
            _promotionRepository = repository;

        }

        public async Task<PromotionDto> GetByCouponCodeAsync(string couponCode)
        {
            var promotion = await _promotionRepository.FirstOrDefaultAsync(p => p.CouponCode == couponCode);

            

            return ObjectMapper.Map<Promotion, PromotionDto>(promotion);
        }

        protected override async Task<IQueryable<Promotion>> CreateFilteredQueryAsync(PagedResultRequestDto input)
        {
            // Tùy chỉnh logic lọc nếu cần thiết
            return await base.CreateFilteredQueryAsync(input);
        }


    }
}
