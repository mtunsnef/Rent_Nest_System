using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RentNest.Core.Domains;
using RentNest.Core.Enums;
using RentNest.Core.Model.PayOS;
using RentNest.Core.UtilHelper;
using RentNest.Infrastructure.DataAccess;

namespace RentNest.Web.Controllers.PayOs
{
    [Route("[controller]")]
    public class PaymentPayOSController : Controller
    {
        private readonly ILogger<PaymentPayOSController> _logger;
        private readonly PostDAO _postDAO;
        private readonly PostPackageDetailDAO _postPackageDetailDAO;
        private readonly PaymentDAO _paymentDAO;
        public PaymentPayOSController(ILogger<PaymentPayOSController> logger, PostDAO postDAO, PostPackageDetailDAO postPackageDetailDAO, PaymentDAO paymentDAO)
        {
            _logger = logger;
            _postDAO = postDAO;
            _postPackageDetailDAO = postPackageDetailDAO;
            _paymentDAO = paymentDAO;
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

        [HttpGet("success")]
        public async Task<IActionResult> PaymentSuccess([FromQuery] int amount, [FromQuery] string transactionId, [FromQuery] int postId)
        {
            try
            {
                var post = await _postDAO.GetByIdAsync(postId);
                if (post == null)
                    return NotFound("Post not found");

                var postPackage = await _postPackageDetailDAO.GetByPostIdAsync(postId);
                if (postPackage == null)
                    return NotFound("Post package not found");
                string packageTypeName = postPackage.Pricing?.PackageType.PackageTypeName ?? "Tin thường";
                var packageType = BadgeHelper.ParsePackageType(packageTypeName);

                if (BadgeHelper.IsVip(packageType))
                {
                    post.CurrentStatus = PostStatusHelper.ToDbValue(PostStatus.Active);
                }
                else
                {
                    post.CurrentStatus = PostStatusHelper.ToDbValue(PostStatus.Pending);
                }

                postPackage.PaymentStatus = PaymentStatusHelper.ToDbValue(PaymentStatus.Completed);
                postPackage.PaymentTransactionId = transactionId;

                var payment = new Payment
                {
                    PostPackageDetailsId = postPackage.Id,
                    TotalPrice = postPackage.TotalPrice,
                    Status = PaymentStatusHelper.ToDbValue(PaymentStatus.Completed),
                    PaymentDate = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    MethodId = 2,
                    AccountId = post.AccountId
                };

                await _paymentDAO.AddAsync(payment);
                await _postDAO.UpdateAsync(post);
                await _postPackageDetailDAO.UpdateAsync(postPackage);

                return Redirect("/payment-success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Payment success processing failed for postId {PostId}", postId);
                return Redirect("/?error=payment_error");
            }
        }


        [HttpGet("/cancel")]
        public async Task<IActionResult> Cancel(int postId, int amount = 0)
        {
            try
            {
                if (postId <= 0)
                {
                    _logger.LogWarning("Cancel payment called without valid postId");
                    return BadRequest("Invalid postId");
                }

                var post = await _postDAO.GetByIdAsync(postId);
                if (post == null)
                {
                    _logger.LogWarning("Post not found for cancellation. postId={PostId}", postId);
                    return NotFound("Post not found");
                }

                var postPackage = await _postPackageDetailDAO.GetByPostIdAsync(postId);
                if (postPackage == null)
                {
                    _logger.LogWarning("PostPackageDetail not found for cancellation. postId={PostId}", postId);
                    return NotFound("Post package not found");
                }

                post.CurrentStatus = PostStatusHelper.ToDbValue(PostStatus.Cancelled);
                postPackage.PaymentStatus = PaymentStatusHelper.ToDbValue(PaymentStatus.Inactive);

                await _postDAO.UpdateAsync(post);
                await _postPackageDetailDAO.UpdateAsync(postPackage);

                var model = new PayOSResponseModel
                {
                    Amount = amount > 0 ? amount : 50000,
                    IsSuccess = false,
                    CreatedAt = DateTime.Now
                };

                _logger.LogInformation("Payment cancelled for postId={PostId}, amount={Amount}", postId, amount);
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