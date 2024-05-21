using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ShopEcommerce.Admin.Orders
{
    public interface IOrdersAppService : ICrudAppService
       <OrderDto,
       Guid,
       PagedResultRequestDto, CreateOrderDto, CreateOrderDto>
    {
        Task<List<OrderDto>> GetAllOrdersAsync();

        // Get a filtered list of orders based on the provided input
        Task<PagedResultDto<OrderDto>> GetListFilterAsync(BaseListFilterDto input);

        // Confirm an order based on its Id
        Task ConfirmOrderAsync(Guid orderId);

        // Delete multiple orders by Ids
        Task DeleteMultipleAsync(IEnumerable<Guid> ids);

        Task<PagedResultDto<ProductSalesDto>> GetProductSalesStatisticsAsync(BaseListFilterDto input);

        Task<List<OrderItemDto>> GetOrderItemsAsync(Guid orderId);

        Task<List<MonthlyRevenueDto>> GetMonthlyRevenueStatisticsForYearAsync(int year);
    }
}
