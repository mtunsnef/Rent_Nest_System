using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentNest.Core.Consts;

namespace RentNest.Web.Controllers
{
    public class RoomController : Controller
    {
        [Route("/Room")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Detail")]
        public IActionResult Detail()
        {
            ViewData["Address"] = "Đ. Nam Kỳ Khởi Nghĩa, Khu đô thị FPT City, Ngũ Hành Sơn, Đà Nẵng 550000";
            return View();
        }

        [Route("/Post")]
        public IActionResult Post()
        {
            return View();
        }
    }
}
