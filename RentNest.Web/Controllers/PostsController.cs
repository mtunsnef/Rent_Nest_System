using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentNest.Core.Consts;
using RentNest.Core.DTO;
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
        public PostsController(IAzureOpenAIService azureOpenAIService, IAccommodationTypeService accommodationTypeService, IAmenitiesSerivce amenitiesService, ITimeUnitPackageService timeUnitPackageService, IPackagePricingService packagePricingService)
        {
            _azureOpenAIService = azureOpenAIService;
            _accommodationTypeService = accommodationTypeService;
            _amenitiesService = amenitiesService;
            _timeUnitPackageService = timeUnitPackageService;
            _packagePricingService = packagePricingService;
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
        public IActionResult ManagePost() { return View("User/ManagePost"); }


    }

}
