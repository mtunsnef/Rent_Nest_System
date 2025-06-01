using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Net.payOS.Types;
using Net.payOS;

namespace RentNest.Web.Controllers.PayOs
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckoutController : Controller
    {
        private readonly PayOS _payOS;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CheckoutController> _logger;


        public CheckoutController(
            PayOS payOS,
            IHttpContextAccessor httpContextAccessor,
            ILogger<CheckoutController> logger)
        {
            _payOS = payOS;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            // Trả về trang HTML có tên "MyView.cshtml"
            return View("index");
        }
        [HttpPost("/create-payment-link")]
        public async Task<IActionResult> Checkout(int Amount)
        {
            Amount = 20000;
            try
            {
                int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                // Sử dụng Amount từ tham số
                ItemData item = new ItemData("Gói đăng ký dịch vụ", 1, Amount);
                List<ItemData> items = new List<ItemData> { item };

                // Get the current request's base URL
                var request = _httpContextAccessor.HttpContext?.Request;
                var baseUrl = $"{request?.Scheme}://{request?.Host}";

                PaymentData paymentData = new PaymentData(
                    orderCode,
                    Amount, // Sử dụng Amount từ tham số
                    "Gói đăng ký dịch vụ",
                    items,
                    $"{baseUrl}/cancel?amount={Amount}",
                    $"{baseUrl}/success?amount={Amount}"
                );

                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

                _logger.LogInformation("Created payment link for amount: {Amount}, OrderCode: {OrderCode}",
                    Amount, orderCode);

                return Redirect(createPayment.checkoutUrl);
            }
            catch (System.Exception exception)
            {
                return Redirect($"/?error=payment_failed");
            }
        }
    }
}