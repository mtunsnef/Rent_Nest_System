namespace RentNest.Web.Models
{
	public class AccommodationDetailViewModel
	{
        //Post
        public string PostTitle { get; set; }

        public string PostContent { get; set; }

		//Accommodation
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

		public string? Title { get; set; } 

		public decimal? Price { get; set; }

		public string? Description { get; set; }

        public List<string> ImageUrls { get; set; } = new List<string>();

        public string Address { get; set; }

		//Owner
		public string? AccountName { get; set; }

		public string? AccountImg { get; set; }

		public string? AccountPhone { get; set; }

		//Amentities

		public List<string> Amenities { get; set; } = new List<string>();

    }

}
