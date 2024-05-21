using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ShopEcommerce.Admin.Promotions
{
    public interface IPromotionAppService : ICrudAppService<
        PromotionDto,
        Guid,
        PagedResultRequestDto,
        CreatePromotionDto,
        CreatePromotionDto>
    {
        // Retrieve a list of all active promotions
        Task<List<PromotionDto>> GetAllPromotionsAsync();

        // Retrieve a filtered list of promotions based on the provided input
        Task<PagedResultDto<PromotionDto>> GetListFilterAsync(BaseListFilterDto input);

        // Activate a specific promotion by its ID
        Task ActivatePromotionAsync(Guid promotionId);

        // Deactivate a specific promotion by its ID
        Task DeactivatePromotionAsync(Guid promotionId);

        // Delete multiple promotions by their IDs
        Task DeleteMultipleAsync(IEnumerable<Guid> ids);

        // Validate a coupon code to check if it can be applied
        Task<bool> ValidateCouponCodeAsync(string couponCode);
    }
}
