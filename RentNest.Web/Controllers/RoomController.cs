using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentNest.Core.Consts;
using RentNest.Core.DTO;
using RentNest.Service.Interfaces;
using System.Collections;
using System.Threading.Tasks;

namespace RentNest.Web.Controllers
{
    // [Authorize(AuthenticationSchemes = AuthSchemes.Cookie, Roles = $"{UserRoles.User}")]
    public class RoomController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IRoomService _roomService;
        public RoomController(IConfiguration configuration, IRoomService roomService)
        {
            _configuration = configuration;
            _roomService = roomService;
        }

        [Route("/Room")]
        public IActionResult Index()
        {
            if (TempData["RoomList"] != null)
            {
                var roomList = JsonConvert.DeserializeObject<List<RoomCardDto>>(TempData["RoomList"].ToString());
                return View(roomList); // Truyền ra view
            }

            return View(new List<RoomCardDto>());
        }

        [Route("/Detail")]
        public IActionResult Detail()
        {
            ViewData["Address"] = "Đ. Nam Kỳ Khởi Nghĩa, Khu đô thị FPT City, Ngũ Hành Sơn, Đà Nẵng 550000";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search([FromForm] string provinceName, string districtName, string wardName, double area, decimal minMoney, decimal maxMoney)
        {
            if (ModelState.IsValid)
            {
                var rooms = await _roomService.GetRoomsBySearchDto(provinceName, districtName, wardName, area, minMoney, maxMoney);
                List<RoomCardDto> roomList = new List<RoomCardDto>();
                foreach (var room in rooms)
                {
                    string status;

                    if (room.Status.Contains("A"))
                    {
                        status = "Available";
                    }
                    else if (room.Status.Contains("I"))
                    {
                        status = "Inactive";
                    }
                    else
                    {
                        status = "Rented";
                    }
                    var roomCart = new RoomCardDto
                    {
                        RoomTitle = room.Title,
                        RoomArea = room.Area,
                        RoomImage = await _roomService.GetRoomImage(room.AccommodationId),
                        RoomPrice = room.Price,
                        roomType = await _roomService.GetRoomType(room.TypeId),
                        RoomAddress = room.Address,
                        RoomStatus = status

                    };
                    roomList.Add(roomCart);
                }
                TempData["RoomList"] = JsonConvert.SerializeObject(roomList);
                return RedirectToAction("Index", "Room");
            }

            return View();
        }

    }
}
