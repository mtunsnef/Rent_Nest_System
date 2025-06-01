using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentNest.Core.Consts;
using RentNest.Core.Domains;
using RentNest.Core.DTO;
using RentNest.Core.UtilHelper;
using RentNest.Service.Implements;
using RentNest.Service.Interfaces;
using RentNest.Web.Models;

namespace RentNest.Web.Controllers
{
    [Authorize(AuthenticationSchemes = AuthSchemes.Cookie, Roles = $"{UserRoles.Landlord}")]
    public class PostsController : Controller
    {
        private readonly IAzureOpenAIService _azureOpenAIService;
        private readonly IAccommodationTypeService _accommodationTypeService;
        private readonly IAmenitiesSerivce _amenitiesService;
        private readonly ITimeUnitPackageService _timeUnitPackageService;
        private readonly IPackagePricingService _packagePricingService;
        private readonly IPostService _postService;
        public PostsController(IAzureOpenAIService azureOpenAIService, IAccommodationTypeService accommodationTypeService, IAmenitiesSerivce amenitiesService, ITimeUnitPackageService timeUnitPackageService, IPackagePricingService packagePricingService, IPostService postService)
        {
            _azureOpenAIService = azureOpenAIService;
            _accommodationTypeService = accommodationTypeService;
            _amenitiesService = amenitiesService;
            _timeUnitPackageService = timeUnitPackageService;
            _packagePricingService = packagePricingService;
            _postService = postService;
        }

        //api
        [HttpGet("/api/v1/package-types/{timeUnitId}")]
        public async Task<IActionResult> GetPackageTypesByTimeUnit(int timeUnitId)
        {
            var result = await _packagePricingService.GetPackageTypes(timeUnitId);
            return Ok(result);
        }
        [HttpGet("/api/v1/durations")]
        public async Task<IActionResult> GetDurations(int timeUnitId, int packageTypeId)
        {
            var result = await _packagePricingService.GetDurationsAndPrices(timeUnitId, packageTypeId);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/v1/get-pricing")]
        public async Task<IActionResult> GetPricingId([FromBody] PricingLookupDto dto)
        {
            var pricingId = await _packagePricingService.GetPricingIdAsync(dto.TimeUnitId, dto.PackageTypeId, dto.DurationValue);

            if (pricingId == null)
                return NotFound(new { message = "Không tìm thấy gói tương ứng." });

            return Ok(new { pricingId });
        }

        //post
        [Route("/nguoi-cho-thue/dang-tin")]
        [HttpGet]
        public async Task<IActionResult> Post()
        {
            var accommodationTypes = await _accommodationTypeService.GetAllAsync();
            var amenities = await _amenitiesService.GetAll();
            var timePackages = await _timeUnitPackageService.GetAll();
            var model = new CreatePostViewModel
            {
                AccommodationTypes = accommodationTypes,
                Amenities = amenities,
                TimeUnitPackages = timePackages
            };
            return View("User/CreatePost", model);
        }


        [Route("/nguoi-cho-thue/dang-tin")]
        [HttpPost]
        public async Task<IActionResult> Post_Landlord([FromForm] LandlordPostDto dto)
        {
            return Json(new { success = dto });
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePostWithAI([FromBody] PostDataAIDto model)
        {
            var content = await _azureOpenAIService.GenerateDataPost(model);
            return Ok(new { content });
        }

        [Route("/quan-ly-tin")]
        public async Task<IActionResult> ManagePost(string status = "P")
        {
            var accountId = User.GetUserId();

            if (accountId == 0) return Unauthorized();

            var allPosts = await _postService.GetAllPostsByUserAsync(accountId.Value);

            var userProfile = allPosts.FirstOrDefault()?.Account?.UserProfile;

            var statusCounts = allPosts
                .GroupBy(p => p.CurrentStatus)
                .ToDictionary(g => g.Key, g => g.Count());

            var filteredPosts = allPosts
                .Where(p => p.CurrentStatus == status)
                .ToList();

            var viewModelList = filteredPosts.Select(f => new ManagePostViewModel
            {
                Id = f.PostId,
                Title = f.Title,
                Address = f.Accommodation.Address,
                DistrictName = f.Accommodation.DistrictName ?? "",
                ProvinceName = f.Accommodation.ProvinceName ?? "",
                WardName = f.Accommodation.WardName ?? "",
                Price = f.Accommodation.Price,
                Status = f.CurrentStatus,
                ImageUrl = f.Accommodation?.AccommodationImages?.FirstOrDefault()?.ImageUrl ?? "",
                Area = f.Accommodation.Area,
                BedroomCount = f.Accommodation.AccommodationDetail?.BedroomCount,
                BathroomCount = f.Accommodation.AccommodationDetail?.BathroomCount,
                CreatedAt = f.CreatedAt,
                PackageTypeName = f.PostPackageDetails
                    .OrderByDescending(p => p.CreatedAt)
                    .Select(p => p.Pricing.PackageType.PackageTypeName)
                    .FirstOrDefault() ?? "",
                RejectionReason = f.PostApprovals
                                .Where(p => p.Status == "R")
                                .OrderByDescending(p => p.ProcessedAt)
                                .Select(p => p.RejectionReason)
                                .FirstOrDefault(),
                AccountName = f.Account.UserProfile.FirstName ?? "" + " " + f.Account.UserProfile.LastName ?? "",
                AvatarUrl = f.Account.UserProfile.AvatarUrl 
            }).ToList();

            ViewBag.CurrentStatus = status;
            ViewBag.StatusCounts = statusCounts;
            ViewBag.AccountName = $"{userProfile?.FirstName} {userProfile?.LastName}".Trim();
            ViewBag.AvatarUrl = userProfile?.AvatarUrl ?? "/images/default-avatar.jpg";

            return View("User/ManagePost", viewModelList);
        }
    }

}
