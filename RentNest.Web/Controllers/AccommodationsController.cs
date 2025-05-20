using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RentNest.Core.Consts;
using RentNest.Core.Domains;
using RentNest.Core.DTO;
using RentNest.Service.Interfaces;
using RentNest.Web.Models;

namespace RentNest.Web.Controllers
{
    public class AccommodationsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAccommodationService _accommodationService;
        private readonly IPostService _postService;
        public AccommodationsController(IConfiguration configuration, IAccommodationService accommodationService, IPostService postService)
        {
            _configuration = configuration;
            _accommodationService = accommodationService;
            _postService = postService;
        }

        [HttpGet]
        [Route("danh-sach-phong-tro")]
        public IActionResult Index()
        {
            var posts = _postService.GetAllPostsWithAccommodation();


            var model = posts.Select(p => new AccommodationIndexViewModel
            {
                Id = p.Accommodation.AccommodationId,
                Status = p.CurrentStatus,
                Title = p.Title,
                Price = p.Accommodation.Price,
                Address = p.Accommodation.Address,
                ImageUrl = p.Accommodation.AccommodationImages?.FirstOrDefault()?.ImageUrl ?? "default-image.jpg"
            }).ToList();

            return View(model);
        }


        [HttpGet("chi-tiet/{id:int}")]
        public IActionResult Detail(int id)
        {

            var detailId = _accommodationService.GetDetailIdByAccommodationId(id);
            Console.WriteLine($"AccommodationId = {id}, DetailId = {detailId}");

            if (detailId == null)
            {
                return Content("No detailId found");
            }

            var room = _accommodationService.GetAccommodationDetailById(detailId.Value);

            if (room == null)
            {
                return Content("Room detail not found for given detailId");
            }


            ViewData["GoogleMapsAPIKey"] = _configuration["GoogleMapsAPIKey"];
            ViewData["Address"] = room.Accommodation?.Address ?? "Đ. Nam Kỳ Khởi Nghĩa, Khu đô thị FPT City, Ngũ Hành Sơn, Đà Nẵng 550000";

            var viewModel = new AccommodationDetailViewModel
            {
                DetailId = room.DetailId,
                HasKitchenCabinet = room.HasKitchenCabinet,
                HasAirConditioner = room.HasAirConditioner,
                HasRefrigerator = room.HasRefrigerator,
                HasWashingMachine = room.HasWashingMachine,
                HasLoft = room.HasLoft,
                FurnitureStatus = room.FurnitureStatus,
                BedroomCount = room.BedroomCount,
                BathroomCount = room.BathroomCount,
                CreatedAt = room.CreatedAt,
                UpdatedAt = room.UpdatedAt,
                AccommodationId = room.AccommodationId,
                Title = room.Accommodation?.Title,
                Price = room.Accommodation?.Price,
                Description = room.Accommodation?.Description,
                ImageUrl = room.Accommodation?.AccommodationImages?.FirstOrDefault()?.ImageUrl ?? "default-image.jpg"
            };

            return View(viewModel);

        }

        [HttpPost]
        public async Task<IActionResult> Search([FromForm] string provinceName, string districtName, string wardName, double area, decimal minMoney, decimal maxMoney)
        {
            if (ModelState.IsValid)
            {
                var rooms = await _accommodationService.GetAccommodationsBySearchDto(provinceName, districtName, wardName, area, minMoney, maxMoney);
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
                        RoomImage = await _accommodationService.GetAccommodationImage(room.AccommodationId),
                        RoomPrice = room.Price,
                        roomType = await _accommodationService.GetAccommodationType(room.TypeId),
                        RoomAddress = room.Address,
                        RoomStatus = status

                    };
                    roomList.Add(roomCart);
                }
                TempData["RoomList"] = JsonConvert.SerializeObject(roomList);
                return RedirectToAction("Index", "Accommodations");
            }

            return View();
        }
    }
}
