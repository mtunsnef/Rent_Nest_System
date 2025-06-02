using Microsoft.AspNetCore.Mvc;

namespace RentNest.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("/trang-chu")]
        public IActionResult Index()
        {
            return View();
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
            ViewData["Email"] = "bluedream.company@email.com";
            ViewData["Phone"] = "(+84) 941 673 660";
            ViewData["Website"] = "blueHouseDaNang.com";

            return View();
        }

        [Route("/bang-gia-tin")]
        public IActionResult PriceTable()
        {
            return View();
        }
    }
}
