using ShopEcommerce.Public;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopEcommerce.Public
{
    public class BaseListFilterDto : PagedResultRequestBase
    {
        public string? Keyword { get; set; }
    }
}