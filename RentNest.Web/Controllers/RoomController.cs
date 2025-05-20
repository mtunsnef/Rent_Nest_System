using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentNest.Core.Consts;
using RentNest.Core.Domains;
using RentNest.Service.Interfaces;
using RentNest.Web.Models;

namespace RentNest.Web.Controllers
{
    [Authorize(AuthenticationSchemes = AuthSchemes.Cookie, Roles = $"{UserRoles.User}")]
	[Route("room")]
	public class RoomController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IRoomService _roomService;
		private readonly IPostService _postService;


        public RoomController(IConfiguration configuration, IRoomService roomService, IPostService postService)
        {
            _configuration = configuration;
            _roomService = roomService;
			_postService = postService;
		}
		[HttpGet("")]
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


		[HttpGet("detail/{id:int}")]
		public IActionResult Detail(int id)
		{

			var detailId = _roomService.GetDetailIdByAccommodationId(id);
			Console.WriteLine($"AccommodationId = {id}, DetailId = {detailId}");

			if (detailId == null)
			{
				return Content("No detailId found");
			}

			var room = _roomService.GetRoomDetailById(detailId.Value);

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

	}
}
