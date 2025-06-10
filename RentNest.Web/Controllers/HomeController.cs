using Microsoft.AspNetCore.Mvc;
using RentNest.Service.Interfaces;
using RentNest.Web.Models;

namespace RentNest.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IPostService _postService;
        private readonly IMailService _mailService;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IPostService postService, IMailService mailService)
        {
            _logger = logger;
            _configuration = configuration;
            _postService = postService;
            _mailService = mailService;
        }

        [HttpGet]
        [Route("/trang-chu")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var posts = await _postService.GetTopVipPostsAsync();

                var viewModel = posts.Select(post => new TopPostViewModel
                {
                    Id = post.PostId,
                    Title = post.Accommodation?.Title ?? "Không rõ",
                    Address = string.Join(", ",
                        new[]
                        {
                    post.Accommodation?.Address,
                    post.Accommodation?.WardName,
                    post.Accommodation?.DistrictName,
                    post.Accommodation?.ProvinceName
                        }.Where(s => !string.IsNullOrWhiteSpace(s))
                    ),
                    ImageUrl = post.Accommodation?.AccommodationImages?.FirstOrDefault()?.ImageUrl ?? "/images/no-image.jpg",
                    AvatarUrl = post.Account?.UserProfile?.AvatarUrl ?? "/images/default-avatar.jpg",
                    Price = post.Accommodation?.Price ?? 0,
                    PackageTypeName = post.PostPackageDetails?
                        .OrderByDescending(d => d.StartDate)?
                        .FirstOrDefault()?.Pricing?.PackageType?.PackageTypeName ?? string.Empty
                }).ToList();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xử lý trang chủ", ex);
            }
        }

        [Route("/ve-chung-toi")]
        public IActionResult About()
        {
            return View();
        }

        [Route("/dich-vu")]
        public IActionResult Service()
        {
            return View();
        }

        [Route("/lien-he")]
        public IActionResult Contact()
        {
            ViewData["GoogleMapsAPIKey"] = _configuration["GoogleMapsAPIKey"];
            ViewData["Address"] = "Đại Học FPT, Hòa Hải, Ngũ Hành Sơn, Da Nang 550000, Vietnam";
            ViewData["Email"] = "bluedream.rentnest.company@gmail.com";
            ViewData["Phone"] = "(+84) 941 673 660";
            ViewData["Website"] = "blueHouseDaNang.com";

            return View();
        }

        [HttpPost]
        [Route("/lien-he")]
        public async Task<IActionResult> Contact(ContactFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["GoogleMapsAPIKey"] = _configuration["GoogleMapsAPIKey"];
                ViewData["Address"] = "Đại Học FPT, Hòa Hải, Ngũ Hành Sơn, Da Nang 550000, Vietnam";
                ViewData["Email"] = "bluedream.rentnest.company@gmail.com";
                ViewData["Phone"] = "(+84) 941 673 660";
                ViewData["Website"] = "blueHouseDaNang.com";
                return View(model);
            }

            var mailToAdmin = new MailContent
            {
                To = _configuration["AppSettings:ContactEmail"],
                Subject = "Liên hệ từ người dùng trên website",
                Body = $@"
            <div style='font-family: Arial, sans-serif;'>
                <h3>Thông tin liên hệ:</h3>
                <p><strong>Họ tên:</strong> {model.FullName}</p>
                <p><strong>Email:</strong> {model.Email}</p>
                <p><strong>Nghề nghiệp:</strong> {model.Occupation}</p>
                <p><strong>Nội dung:</strong><br/>{model.Message}</p>
                <br/>
                <p>Được gửi từ trang liên hệ của website RentNest</p>
            </div>"
            };

            var mailToCustomer = new MailContent
            {
                To = model.Email,
                Subject = "Cảm ơn bạn đã liên hệ với RentNest",
                Body = $@"
            <div style='font-family: Arial, sans-serif; color: #333; line-height: 1.6;'>
                <p>Xin chào <strong>{model.FullName}</strong>,</p>
                <p>Cảm ơn bạn đã liên hệ với RentNest!</p>
                <p>Chúng tôi rất vui khi nhận được thông tin từ bạn và sẽ nhanh chóng phản hồi trong thời gian sớm nhất.</p>
                <p>Nếu bạn cần hỗ trợ gấp hoặc có thắc mắc gì, đừng ngần ngại trả lời email này hoặc gọi cho chúng tôi nhé.</p>
                <p>Chúc bạn một ngày tuyệt vời!</p>
                <br/>
                <p>Trân trọng,<br/><strong>Đội ngũ RentNest</strong></p>
            </div>"
            };

            var successAdmin = await _mailService.SendMail(mailToAdmin);
            var successCustomer = await _mailService.SendMail(mailToCustomer);

            if (successAdmin && successCustomer)
            {
                TempData["Message"] = "Gửi liên hệ thành công! Chúng tôi sẽ phản hồi sớm nhất.";
                return RedirectToAction("Contact");
            }

            ModelState.AddModelError("", "Có lỗi xảy ra khi gửi email. Vui lòng thử lại sau.");
            return View(model);
        }


        [Route("/bang-gia-tin")]
        public IActionResult PriceTable()
        {
            return View();
        }
    }
}
