using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ShopEcommerce.Public.Promotions
{
    public interface IPromotionAppService : ICrudAppService<
        PromotionDto,
        Guid,
        PagedResultRequestDto,
        CreatePromotionDto,
        CreatePromotionDto>
    {
        Task<PromotionDto> GetByCouponCodeAsync(string couponCode);
    }
}
