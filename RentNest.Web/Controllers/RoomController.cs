using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentNest.Core.Consts;

namespace RentNest.Web.Controllers
{
    [Authorize(AuthenticationSchemes = AuthSchemes.Cookie, Roles = $"{UserRoles.User}")]
    public class RoomController : Controller
    {
        private readonly IConfiguration _configuration;

        public RoomController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("/Room")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Detail")]
        public IActionResult Detail()
        {
            ViewData["GoogleMapsAPIKey"] = _configuration["GoogleMapsAPIKey"];
            ViewData["Address"] = "Đ. Nam Kỳ Khởi Nghĩa, Khu đô thị FPT City, Ngũ Hành Sơn, Đà Nẵng 550000";
            return View();
        }
    }
}
