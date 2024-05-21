using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopEcommerce.Admin.Catalog.Products.Attributes
{
    public class AddUpdateProductAttributeDtoValidator : AbstractValidator<AddUpdateProductAttributeDto>
    {
        public AddUpdateProductAttributeDtoValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.AttributeId).NotEmpty();
        }
    }
}