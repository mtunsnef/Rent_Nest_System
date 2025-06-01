using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
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
        private readonly IFavoriteService _favoriteService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccommodationsController(IConfiguration configuration, IAccommodationService accommodationService, IPostService postService, IFavoriteService favoriteService,
                                        IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _accommodationService = accommodationService;
            _postService = postService;
            _favoriteService = favoriteService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("danh-sach-phong-tro")]
        public async Task<IActionResult> Index()
        {
            var posts = await _postService.GetAllPostsWithAccommodation();
            var model = posts.Select(p => new AccommodationIndexViewModel
            {
                Id = p.PostId,
                Status = p.CurrentStatus,
                Title = p.Title,
                Price = p.Accommodation.Price,
                Address = p.Accommodation.Address,
                Area = p.Accommodation.Area,
                BathroomCount = p.Accommodation?.AccommodationDetail?.BathroomCount,
                BedroomCount = p.Accommodation?.AccommodationDetail?.BedroomCount,
                ImageUrl = p.Accommodation?.AccommodationImages?.FirstOrDefault()?.ImageUrl ?? "default-image.jpg",
                CreatedAt = p.CreatedAt
            }).ToList();

            return View(model);
        }

        public IActionResult FavoritePosts()
        {
            var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var favoritePosts = _favoriteService.GetFavoriteByUser(accountId);

            var viewModelList = favoritePosts.Select(f => new AccommodationIndexViewModel
            {
                Id = f.Post.PostId,
                Title = f.Post.Accommodation.Title,
                Address = f.Post.Accommodation.Address,
                Price = f.Post.Accommodation.Price,
                Status = f.Post.Accommodation.Status,
                ImageUrl = f.Post.Accommodation?.AccommodationImages?.FirstOrDefault()?.ImageUrl ?? "default-image.jpg",
                Area = f.Post.Accommodation.Area,
                BedroomCount = f.Post.Accommodation.AccommodationDetail?.BedroomCount,
                BathroomCount = f.Post.Accommodation.AccommodationDetail?.BathroomCount,
                CreatedAt = f.Post.CreatedAt
            }).ToList();

            return View("FavoritePosts", viewModelList);
        }



        [HttpGet("chi-tiet/{id:int}")]
        public async Task<IActionResult> Detail(int id)
        {
            var accommodationId = await _postService.GetAccommodationIdByPostId(id);
            if (accommodationId == null)
            {
                return Content("AccommodationId not found for given PostId");
            }
            var detailId = _accommodationService.GetDetailIdByAccommodationId(accommodationId.Value);
            Console.WriteLine($"This is AccommodationId = {id}, DetailId = {detailId}");

            if (detailId == null)
            {
                return Content("No detailId found");
            }

            var room = _accommodationService.GetAccommodationDetailById(detailId.Value);

            if (room == null)
            {
                return Content("Room detail not found for given detailId");
            }

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
                PostId = id,
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
                HttpContext.Session.SetObject("FilteredRooms", rooms);
                rooms = rooms.Where(r => r.Status.Contains("A")).ToList();
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

        [HttpGet]
        public IActionResult IsFavorite(int postId)
        {
            var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var isFavorite = _favoriteService.IsFavorite(postId, accountId);
            return Json(isFavorite);
        }

        [HttpPost]
        public IActionResult AddToFavorite(int postId)
        {
            var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            Console.WriteLine($"Adding favorite - Account: {accountId}, Post: {postId}");

            if (accountId == 0)
            {
                return Unauthorized();
            }

            _favoriteService.AddToFavorite(postId, accountId);
            return Ok();
        }

        [HttpPost]
        public IActionResult RemoveFromFavorite(int postId)
        {
            var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (accountId == 0)
            {
                return Unauthorized();
            }

            _favoriteService.RemoveFromFavorite(postId, accountId);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> FilterRooms(string roomType, string roomStatus, int? minPrice, int? maxPrice, int? minArea, int? maxArea)
        {
            var rooms = HttpContext.Session.GetObject<List<Accommodation>>("FilteredRooms");

            if (!string.IsNullOrEmpty(roomType))
            {
                // Duyệt từng phòng và lọc theo loại phòng bất đồng bộ
                var filteredByType = new List<Accommodation>();
                foreach (var r in rooms)
                {
                    var typeName = await _accommodationService.GetAccommodationType(r.TypeId);
                    if (typeName == roomType)
                    {
                        filteredByType.Add(r);
                    }
                }
                rooms = filteredByType;
            }

            if (!string.IsNullOrEmpty(roomStatus))
                rooms = rooms!.Where(r => r.Status == roomStatus).ToList();

            if (minPrice.HasValue)
                rooms = rooms!.Where(r => r.Price >= minPrice).ToList();

            if (maxPrice.HasValue)
                rooms = rooms!.Where(r => r.Price <= maxPrice).ToList();

            if (minArea.HasValue)
                rooms = rooms!.Where(r => r.Area >= minArea).ToList();

            if (maxArea.HasValue)
                rooms = rooms!.Where(r => r.Area <= maxArea).ToList();

            return PartialView("_RoomListPartial", rooms);
        }
    }

}
