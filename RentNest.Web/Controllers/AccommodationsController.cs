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
        private readonly IAccommodationService _accommodationService;
        private readonly IPostService _postService;
        private readonly IAmenitiesSerivce _amenitiesSerivce;
        private readonly IFavoriteService _favoriteService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccommodationsController(IConfiguration configuration, IAccommodationService accommodationService, IPostService postService,
        IFavoriteService favoriteService, IAmenitiesSerivce amenitiesSerivce, IHttpContextAccessor httpContextAccessor)
        {
            _accommodationService = accommodationService;
            _postService = postService;
            _amenitiesSerivce = amenitiesSerivce;
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
        [Authorize(AuthenticationSchemes = AuthSchemes.Cookie, Roles = $"{UserRoles.User},{UserRoles.Landlord}")]
        [Route("bai-viet-yeu-thich")]
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

        [HttpGet("chi-tiet/{postId:int}")]
        public async Task<IActionResult> Detail(int postId)
        {
            var post = await _postService.GetPostDetailWithAccommodationDetailAsync(postId);

            if (post == null || post.Accommodation == null || post.Accommodation.AccommodationDetail == null)
            {
                return Content("Không tìm thấy dữ liệu chi tiết");
            }

            var imageUrls = post.Accommodation.AccommodationImages?
                .Select(img => img.ImageUrl)
                .ToList() ?? new List<string>();

            var viewModel = new AccommodationDetailViewModel
            {
                PostId = post.PostId,
                PostTitle = post.Title,
                PostContent = post.Content,
                DetailId = post.Accommodation.AccommodationDetail.DetailId,
                AccommodationId = post.Accommodation.AccommodationId,
                ImageUrls = imageUrls,
                Price = post.Accommodation.Price,
                Description = post.Accommodation.Description,
                BathroomCount = post.Accommodation.AccommodationDetail.BathroomCount,
                BedroomCount = post.Accommodation.AccommodationDetail.BedroomCount,
                HasKitchenCabinet = post.Accommodation.AccommodationDetail.HasKitchenCabinet,
                HasAirConditioner = post.Accommodation.AccommodationDetail.HasAirConditioner,
                HasRefrigerator = post.Accommodation.AccommodationDetail.HasRefrigerator,
                HasWashingMachine = post.Accommodation.AccommodationDetail.HasWashingMachine,
                HasLoft = post.Accommodation.AccommodationDetail.HasLoft,
                FurnitureStatus = post.Accommodation.AccommodationDetail.FurnitureStatus,
                CreatedAt = post.Accommodation.AccommodationDetail.CreatedAt,
                UpdatedAt = post.Accommodation.AccommodationDetail.UpdatedAt,
                Address = post.Accommodation.Address ?? "",
                AccountImg = post.Account.UserProfile.AvatarUrl,
                AccountName = post.Account.UserProfile.FirstName + " " + post.Account.UserProfile.LastName,
                AccountPhone = post.Account.UserProfile.PhoneNumber,
                Amenities = post.Accommodation.AccommodationAmenities?
                    .Where(a => a.Amenity != null)
                    .Select(a => a.Amenity.AmenityName)
                    .ToList() ?? new List<string>()
            };

            ViewData["Address"] = viewModel.Address;

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

        [HttpGet]
        public IActionResult IsFavorite(int postId)
        {
            var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var isFavorite = _favoriteService.IsFavorite(postId, accountId);
            return Json(isFavorite);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = AuthSchemes.Cookie, Roles = $"{UserRoles.User},{UserRoles.Landlord}")]
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
        [Authorize(AuthenticationSchemes = AuthSchemes.Cookie, Roles = $"{UserRoles.User},{UserRoles.Landlord}")]
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
    }
}
