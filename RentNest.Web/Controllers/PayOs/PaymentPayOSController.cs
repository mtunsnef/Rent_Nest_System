using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RentNest.Core.Model.PayOS;

namespace RentNest.Web.Controllers.PayOs
{
    [Route("[controller]")]
    public class PaymentPayOSController : Controller
    {
        private readonly ILogger<PaymentPayOSController> _logger;

        public PaymentPayOSController(ILogger<PaymentPayOSController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
        [HttpGet("/success")]
        public IActionResult Success(int amount = 0)
        {
            try
            {
                var model = new PayOSResponseModel
                {
                    Amount = amount > 0 ? amount : 50000,
                    IsSuccess = true,
                    CreatedAt = DateTime.Now
                };

                _logger.LogInformation("Payment success for amount: {Amount}", amount);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing success payment");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet("/cancel")]
        public IActionResult Cancel(int amount = 0)
        {
            try
            {
                var model = new PayOSResponseModel
                {
                    Amount = amount > 0 ? amount : 50000,
                    IsSuccess = false,
                    CreatedAt = DateTime.Now
                };

                _logger.LogInformation("Payment cancelled for amount: {Amount}", amount);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing cancelled payment");
                return RedirectToAction("Error", "Home");
            }
        }
    }
}