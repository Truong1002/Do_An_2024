using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopEcommerce.Orders;
using ShopEcommerce.Products;
using ShopEcommerce.Public.Products;
using ShopEcommerce.Public.Promotions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ShopEcommerce.Public.Orders
{
    public class OrdersAppService : CrudAppService<
        Order,
        OrderDto,
        Guid,
        PagedResultRequestDto, CreateOrderDto, CreateOrderDto>, IOrdersAppService
    {
        private readonly IRepository<OrderItem> _orderItemRepository;
        private readonly OrderCodeGenerator _orderCodeGenerator;
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IPromotionAppService _promotionAppService;
        private readonly IProductsAppService _productsAppService;


        public OrdersAppService(IRepository<Order, Guid> repository,
            OrderCodeGenerator orderCodeGenerator,
            IRepository<OrderItem> orderItemRepository,
            IRepository<Product, Guid> productRepository,
            IPromotionAppService promotionAppService,
            IProductsAppService productsAppService)
            : base(repository)
        {
            _orderItemRepository = orderItemRepository;
            _orderCodeGenerator = orderCodeGenerator;
            _productRepository = productRepository;
            _promotionAppService = promotionAppService;
            _productsAppService = productsAppService;
            
        }

        public override async Task<OrderDto> CreateAsync(CreateOrderDto input)
        {
            var subTotal = input.Items.Sum(x => x.Quantity * x.Price);

            var discount = 0.0;
            if (!string.IsNullOrEmpty(input.CouponCode))
            {
                var promotion = await _promotionAppService.GetByCouponCodeAsync(input.CouponCode);
                if (promotion != null && promotion.IsActive)
                {
                    // Giảm giá bằng % của tổng đơn hàng
                    discount = input.Discount;
                }
            }
            var grandTotal = subTotal - discount;
            var orderId = Guid.NewGuid();
            var order = new Order(orderId)
            {
                Code = await _orderCodeGenerator.GenerateAsync(),
                CustomerAddress = input.CustomerAddress,
                CustomerName = input.CustomerName,
                CustomerPhoneNumber = input.CustomerPhoneNumber,
                ShippingFee = 0,
                CustomerUserId = input.CustomerUserId,
                Tax = 0,
                Subtotal = subTotal,
                GrandTotal = grandTotal,
                Discount = discount,
                PaymentMethod = PaymentMethod.COD,
                Total = subTotal,
                Status = OrderStatus.New

            };
            var items = new List<OrderItem>();
            foreach (var item in input.Items)
            {
                var product = await _productRepository.GetAsync(item.ProductId);
                items.Add(new OrderItem()
                {
                    OrderId = orderId,
                    Price = item.Price,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    SKU = product.SKU
                });
            }
            await _orderItemRepository.InsertManyAsync(items);

            var result = await Repository.InsertAsync(order);


            return ObjectMapper.Map<Order, OrderDto>(result);
        }

        public async Task<List<OrderDto>> GetCustomerOrdersAsync(Guid customerUserId)
        {
            // Query orders based on CustomerUserId and sort by CreationTime descending
            var orders = await Repository.GetListAsync(order => order.CustomerUserId == customerUserId);

            // Sort orders by CreationTime descending (latest first)
            var sortedOrders = orders.OrderByDescending(order => order.CreationTime);

            // Transform the sorted list into OrderDto
            return sortedOrders.Select(order => new OrderDto
            {
                Id = order.Id,
                Code = order.Code,
                CustomerAddress = order.CustomerAddress,
                CustomerName = order.CustomerName,
                CustomerPhoneNumber = order.CustomerPhoneNumber,
                OrderDate = order.CreationTime,
                Status = order.Status,
                Total = order.Total,
                Discount = order.Discount,
                GrandTotal = order.GrandTotal,
            }).ToList();
        }

        public async Task<List<OrderItemDto>> GetOrderItemsAsync(Guid orderId)
        {
            var orderItems = await _orderItemRepository.GetListAsync(item => item.OrderId == orderId);
            var productIds = orderItems.Select(i => i.ProductId).Distinct().ToList();
            var products = await _productRepository.GetListAsync(p => productIds.Contains(p.Id));

            var productDictionary = products.ToDictionary(p => p.Id, p => p);

            var orderItemDtos = new List<OrderItemDto>();

            foreach (var item in orderItems)
            {
                var orderItemDto = new OrderItemDto
                {
                    OrderId = item.OrderId,
                    ProductId = item.ProductId,
                    SKU = item.SKU,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    ProductName = productDictionary[item.ProductId].Name,
                    ProductImageUrl = productDictionary[item.ProductId].ThumbnailPicture
                };

                if (!string.IsNullOrEmpty(orderItemDto.ProductImageUrl))
                {
                    var base64Image = await _productsAppService.GetThumbnailImageAsync(orderItemDto.ProductImageUrl);
                    if (!string.IsNullOrEmpty(base64Image))
                    {
                        // Đảm bảo rằng chuỗi base64 được định dạng đúng cho việc sử dụng trong thẻ img
                        orderItemDto.ProductImageUrlBase64 = $"data:image/jpeg;base64,{base64Image}";
                    }
                }

                orderItemDtos.Add(orderItemDto);
            }

            return orderItemDtos;
        }


        public async Task<OrderDto> UpdateAsync(Guid id, OrderDto input)
        {
            var existingOrder = await Repository.GetAsync(id);
            if (existingOrder == null)
            {
                throw new KeyNotFoundException("Không tìm thấy đơn hàng với ID đã cho.");
            }

            // Cập nhật thuộc tính đơn hàng
            existingOrder.CustomerAddress = input.CustomerAddress;
            existingOrder.CustomerName = input.CustomerName;
            existingOrder.CustomerPhoneNumber = input.CustomerPhoneNumber;
            existingOrder.Status = input.Status;
            existingOrder.PaymentMethod = input.PaymentMethod;
            existingOrder.ShippingFee = input.ShippingFee;
            existingOrder.Tax = input.Tax;
            existingOrder.Total = input.Total;
            existingOrder.Subtotal = input.Subtotal;
            existingOrder.Discount = input.Discount;
            existingOrder.GrandTotal = input.GrandTotal;

            // Lưu thay đổi
            await Repository.UpdateAsync(existingOrder);

            return ObjectMapper.Map<Order, OrderDto>(existingOrder);
        }


    }
}