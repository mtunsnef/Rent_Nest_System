namespace RentNest.Web.Models
{
	public class AccommodationDetailViewModel
	{
		public int DetailId { get; set; }

		public bool? HasKitchenCabinet { get; set; }

		public bool? HasAirConditioner { get; set; }

		public bool? HasRefrigerator { get; set; }

		public bool? HasWashingMachine { get; set; }

		public bool? HasLoft { get; set; }

		public string? FurnitureStatus { get; set; }

		public int? BedroomCount { get; set; }

		public int? BathroomCount { get; set; }

		public DateTime? CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

		public int AccommodationId { get; set; }	

		public string? Title { get; set; } // Optional for display use
		public decimal? Price { get; set; }
		public string? Description { get; set; }
		public string ImageUrl { get; set; }

	}

}
