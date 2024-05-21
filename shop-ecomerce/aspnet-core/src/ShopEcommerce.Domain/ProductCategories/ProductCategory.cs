using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace ShopEcommerce.ProductCategories
{
    public class ProductCategory : CreationAuditedAggregateRoot<Guid>
    {
        public required string Name { get; set; }
        public required string Code { get; set; }
        public required string Slug { get; set; }
        public required int SortOrder { get; set; }
        public string? CoverPicture { get; set; }
        public required bool Visibility { get; set; }
        public required bool IsActive { get; set; }
        public Guid? ParentId { get; set; }
        public string? SeoMetaDescription { get; set;}
    }
}
