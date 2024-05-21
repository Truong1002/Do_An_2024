using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopEcommerce.Orders;
using ShopEcommerce.Public.Orders;
using ShopEcommerce.Public.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopEcommerce.Public.Web.Pages.Cart
{
    public class MyorderModel : PageModel
    {
        private readonly IOrdersAppService _ordersAppService;

        public MyorderModel(IOrdersAppService ordersAppService)
        {
            _ordersAppService = ordersAppService;
        }

        public List<OrderDto> Orders { get; set; }
        public Dictionary<Guid, List<OrderItemDto>> OrderItems { get; set; } = new Dictionary<Guid, List<OrderItemDto>>();

        public async Task OnGetAsync()
        {
            Guid? currentUserId = User.Identity.IsAuthenticated ? User.GetUserId() : null;

            if (currentUserId.HasValue)
            {
                Orders = await _ordersAppService.GetCustomerOrdersAsync(currentUserId.Value);
                foreach (var order in Orders)
                {
                    var items = await _ordersAppService.GetOrderItemsAsync(order.Id);
                    OrderItems[order.Id] = items;
                }
            }
            else
            {
                // Handle the case where the user is not authenticated or the ID is invalid
                Orders = new List<OrderDto>();
            }
        }

        public async Task<IActionResult> OnPostCancelOrderAsync(Guid OrderId)
        {
            // Cập nhật trạng thái đơn hàng thành "Cancelled"
            var order = await _ordersAppService.GetAsync(OrderId);
            if (order.Status == OrderStatus.New)
            {
                order.Status = OrderStatus.Canceled;
                await _ordersAppService.UpdateAsync(OrderId, order);
            }

            // Refresh orders to update the page
            return RedirectToPage();
        }
    }
}
