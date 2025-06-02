using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Net.payOS.Types;
using Net.payOS;
using RentNest.Infrastructure.DataAccess;

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


        [HttpPost("create-payment-link")]
        public async Task<IActionResult> CheckoutPayment([FromForm] int postId, [FromForm] int AmountPayOs)
        {
            try
            {
                int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));

                ItemData item = new ItemData("Gói đăng ký dịch vụ", 1, AmountPayOs);
                List<ItemData> items = new List<ItemData> { item };

                var request = _httpContextAccessor.HttpContext?.Request;
                var baseUrl = $"{request?.Scheme}://{request?.Host}";

                PaymentData paymentData = new PaymentData(
                    orderCode,
                    AmountPayOs,
                    "Gói đăng ký dịch vụ",
                    items,
                    $"{baseUrl}/cancel?postId={postId}&amount={AmountPayOs}",
                    $"{baseUrl}/success?postId={postId}&amount={AmountPayOs}"
                );

                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

                _logger.LogInformation("Created payment link for PostId: {PostId}, OrderCode: {OrderCode}",
                    postId, orderCode);

                return Redirect(createPayment.checkoutUrl);
            }
            catch (System.Exception exception)
            {
                _logger.LogError(exception, "Checkout payment failed for PostId: {PostId}", postId);
                return Redirect($"/?error=payment_failed");
            }
        }

    }
}