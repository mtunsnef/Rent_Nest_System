using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace RentNest.Core.DTO
{
    public class LandlordPostDto
    {
        public required string Address { get; set; }
        public required int AccommodationTypeId { get; set; }
        public required int Area { get; set; }
        public required decimal Price { get; set; }
        public string? FurnitureStatus { get; set; }
        public int? NumberBedroom { get; set; }
        public int? NumberBathroom { get; set; }
        public bool? HasKitchenCabinet { get; set; }
        public bool? HasAirConditioner { get; set; }
        public bool? HasRefrigerator { get; set; }
        public bool? HasWashingMachine { get; set; }
        public bool? HasLoft { get; set; }
        public required string titlePost { get; set; }
        public required string contentPost { get; set; }
        public List<int>? AmenityIds { get; set; }
        public List<IFormFile>? Images { get; set; }
        public int? PricingId { get; set; }
        public int? OwnerId { get; set; }
        public string? AccommodationDescription { get; set; }
        public double TotalPrice { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}
