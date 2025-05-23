using Microsoft.AspNetCore.Mvc;

namespace RentNest.Web.Controllers
{
    public class ChatRoomController : Controller
    {
        [Route("tro-chuyen")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
