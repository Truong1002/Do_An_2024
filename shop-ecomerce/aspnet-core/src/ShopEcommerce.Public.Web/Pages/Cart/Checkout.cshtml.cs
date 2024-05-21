using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopEcommerce.Public.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ShopEcommerce.Public.Orders;
using ShopEcommerce.Public.Web.Extensions;
using ShopEcommerce.Public.Web.Models;
using Volo.Abp.TextTemplating;
using Volo.Abp.Emailing;
using ShopEcommerce.Emailing;
using System.Security.Claims;
using ShopEcommerce.Public.Promotions;

namespace ShopEcommerce.Public.Web.Pages.Cart
{
    public class CheckoutModel : PageModel
    {
        private readonly IOrdersAppService _ordersAppService;
        private readonly IEmailSender _emailSender;
        private readonly ITemplateRenderer _templateRenderer;
        private readonly IPromotionAppService _promotionAppService;
        public CheckoutModel(IOrdersAppService ordersAppService, IEmailSender emailSender,
           ITemplateRenderer templateRenderer,
            IPromotionAppService promotionAppService)
        {
            _ordersAppService = ordersAppService;
            _emailSender = emailSender;
            _templateRenderer = templateRenderer;
            _promotionAppService = promotionAppService;
        }
        public List<CartItem> CartItems { get; set; }

        public bool? CreateStatus { set; get; }

        [BindProperty]
        public OrderDto Order { set; get; }

        [BindProperty]
        public string CouponCode { get; set; }

        public double DiscountApplied { get; set; }
        public double TotalAfterDiscount { get; set; }

        public void OnGet()
        {
            
            CartItems = GetCartItems();
        }

        /* public async Task<IActionResult> OnPostApplyCouponAsync()
         {
             if (!string.IsNullOrEmpty(CouponCode))
             {
                 var promotion = await _promotionAppService.GetByCouponCodeAsync(CouponCode);
                 if (promotion != null && promotion.IsActive)
                 {
                     CartItems = GetCartItems();
                     // Áp dụng giảm giá vào từng sản phẩm trong giỏ hàng
                     foreach (var item in CartItems)
                     {
                         double discount = item.Product.SellPrice * (promotion.DiscountAmount);
                         item.Product.SellPrice -= discount;
                     }
                 }
             }
             return Page();
         }*/
        public async Task<IActionResult> OnPostApplyCouponAsync()
        {
            if (!string.IsNullOrEmpty(CouponCode))
            {
               
                var promotion = await _promotionAppService.GetByCouponCodeAsync(CouponCode);
                if(promotion == null)
                {
                    TempData["CouponError"] = "Mã giảm giá không hợp lệ hoặc đã hết hạn.";
                }
                CartItems = GetCartItems();
                if (promotion != null && promotion.IsActive && promotion.ValidDate <= DateTime.Now && (!promotion.ExpiredDate.HasValue || promotion.ExpiredDate.Value >= DateTime.Now))
                {
                    double total = CartItems.Sum(item => item.Product.SellPrice * item.Quantity);
                    double discount = total * (promotion.DiscountAmount); // Giả sử DiscountAmount là phần trăm
                    DiscountApplied = discount;
                    TotalAfterDiscount = total - discount;

                    ViewData["DiscountAmount"] = DiscountApplied.ToString("N0");
                    ViewData["TotalAfterDiscount"] = TotalAfterDiscount.ToString("N0");
                    ViewData["Subtotal"] = total.ToString("N0");

                    HttpContext.Session.SetString("TotalAfterDiscount", TotalAfterDiscount.ToString());
                    HttpContext.Session.SetString("Discount", DiscountApplied.ToString());
                    HttpContext.Session.SetString("CouponCode", CouponCode);


                    TempData["CouponApplied"] = $"Áp dụng thành công mã giảm giá. Bạn được giảm {discount.ToString("N0")} đ.";
                }
                else
                {
                    TempData["CouponError"] = "Mã giảm giá không hợp lệ hoặc đã hết hạn.";
                }
            }
            CartItems = GetCartItems();
            return Page();
        }

        /*public async Task<IActionResult> OnPostPlaceOrderAsync()
        {

            if (!ModelState.IsValid)
            {

            }


            var cartItems = new List<OrderItemDto>();
            double total = 0; // Initialize total amount
            string productDetails = ""; // Initialize product details string
            foreach (var item in GetCartItems())
            {
                cartItems.Add(new OrderItemDto()
                {
                    Price = item.Product.SellPrice,
                    ProductId = item.Product.Id,
                    Quantity = item.Quantity
                });
                total += item.Product.SellPrice * item.Quantity;
                productDetails += $"{item.Product.Name} (Số lượng: {item.Quantity} x {item.Product.SellPrice.ToString("N0")})\n";
            }
            // Retrieve discount from session or calculate again if needed
            double discount = HttpContext.Session.GetString("TotalAfterDiscount") != null
                ? double.Parse(HttpContext.Session.GetString("TotalAfterDiscount")) - total
                : 0;

            string totalAfterDiscountString = HttpContext.Session.GetString("TotalAfterDiscount");
            if (!string.IsNullOrEmpty(totalAfterDiscountString))
            {
                if (double.TryParse(totalAfterDiscountString, out double totalAfterDiscount))
                {
                    total = totalAfterDiscount;
                }
            }
            Guid? currentUserId = User.Identity.IsAuthenticated ? User.GetUserId() : null;
            if (currentUserId == null)
            {
                return Redirect("/login.html");
            }

            var order = await _ordersAppService.CreateAsync(new CreateOrderDto()
            {
                CustomerName = Order.CustomerName,
                CustomerAddress = Order.CustomerAddress,
                CustomerPhoneNumber = Order.CustomerPhoneNumber,
                Items = cartItems,
                CustomerUserId = currentUserId,
                Discount = discount,
                CouponCode = CouponCode,
            });
            CartItems = GetCartItems();

            if (order != null)
            {
                *//*if (User.Identity.IsAuthenticated)
                {
                    var email = User.GetSpecificClaim(ClaimTypes.Email);
                    var totalAsString = total.ToString("N0");
                    var emailBody = await _templateRenderer.RenderAsync(
                        EmailTemplates.CreateOrderEmail,
                        new
                        {
                            message = $"{productDetails}\nTổng tiền là: {totalAsString}",
                        });
                    await _emailSender.SendAsync(email, "Tạo đơn hàng thành công", emailBody);
                    HttpContext.Session.Remove(ShopEcommerceConsts.Cart);

                }*//*
                CreateStatus = true;
            }

            else
                CreateStatus = false;

            return Page();
        }*/

        public async Task<IActionResult> OnPostPlaceOrderAsync()
        {

            if (!ModelState.IsValid)
            {

            }


            var cartItems = new List<OrderItemDto>();
            double total = 0; // Initialize total amount
            string productDetails = ""; // Initialize product details string
            foreach (var item in GetCartItems())
            {
                cartItems.Add(new OrderItemDto()
                {
                    Price = item.Product.SellPrice,
                    ProductId = item.Product.Id,
                    Quantity = item.Quantity
                });
                total += item.Product.SellPrice * item.Quantity;
                productDetails += $"{item.Product.Name} (Số lượng: {item.Quantity} x {item.Product.SellPrice.ToString("N0")})\n";
            }
            double.TryParse(HttpContext.Session.GetString("TotalAfterDiscount"), out double totalAfterDiscount);
            double.TryParse(HttpContext.Session.GetString("Discount"), out double totalDiscount);
            double discount = total - totalAfterDiscount;
            string couponCode = HttpContext.Session.GetString("CouponCode");

           
            Guid? currentUserId = User.Identity.IsAuthenticated ? User.GetUserId() : null;
            if (currentUserId == null)
            {
                return Redirect("/login.html");
            }
            var orderDto = new CreateOrderDto()
            {
                CustomerName = Order.CustomerName,
                CustomerAddress = Order.CustomerAddress,
                CustomerPhoneNumber = Order.CustomerPhoneNumber,
                Items = cartItems,
                CustomerUserId = currentUserId,
                Discount = discount,
                CouponCode = couponCode,
            };

            var order = await _ordersAppService.CreateAsync(orderDto);
            CartItems = GetCartItems();

            if (order != null)
            {
                /*if (User.Identity.IsAuthenticated)
                {
                    var email = User.GetSpecificClaim(ClaimTypes.Email);
                    var totalAsString =(total- totalAfterDiscount).ToString("N0");
                    var discountAsString = totalDiscount.ToString("N0");
                    var emailBody = await _templateRenderer.RenderAsync(
                        EmailTemplates.CreateOrderEmail,
                        new
                        {
                            message = $"{productDetails}\nĐược giảm: {discountAsString}\nTiền phải trả là: {totalAsString}",
                        });
                    await _emailSender.SendAsync(email, "Tạo đơn hàng thành công", emailBody);
                    HttpContext.Session.Remove(ShopEcommerceConsts.Cart);

                }*/
                CreateStatus = true;
                if (CreateStatus == true)
                {
                    HttpContext.Session.Remove(ShopEcommerceConsts.Cart);
                    HttpContext.Session.Remove("TotalAfterDiscount");
                    HttpContext.Session.Remove("CouponCode");
                }
            }

            else
                CreateStatus = false;

            return Page();
        }


        private List<CartItem> GetCartItems()
        {
            var cart = HttpContext.Session.GetString(ShopEcommerceConsts.Cart);
            var productCarts = new Dictionary<string, CartItem>();
            if (cart != null)
            {
                productCarts = JsonSerializer.Deserialize<Dictionary<string, CartItem>>(cart);
            }
            return productCarts.Values.ToList();
        }

    }
}