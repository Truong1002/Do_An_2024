using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ShopEcommerce.Public.Orders
{
    public interface IOrdersAppService : ICrudAppService
        <OrderDto,
        Guid,
        PagedResultRequestDto, CreateOrderDto, CreateOrderDto>
    {
        Task<List<OrderDto>> GetCustomerOrdersAsync(Guid customerUserId);
        Task<OrderDto> UpdateAsync(Guid id, OrderDto input);
        Task<List<OrderItemDto>> GetOrderItemsAsync(Guid orderId);
    }
}