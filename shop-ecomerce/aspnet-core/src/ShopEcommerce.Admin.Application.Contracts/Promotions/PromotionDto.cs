using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ShopEcommerce.Admin.Promotions
{
    public class PromotionDto : EntityDto<Guid>

    {
        public string? Name { get; set; }
        public string? CouponCode { get; set; }
        public bool RequireUseCouponCode { get; set; }
        public DateTime ValidDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public double DiscountAmount { get; set; }
        public bool LimitedUsageTimes { get; set; }
        public uint MaximumDiscountAmount { get; set; }
        public bool IsActive { get; set; }
    }
}
