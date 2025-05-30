namespace RentNest.Web.Models
{
    public class AccommodationIndexViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public decimal? Price { get; set; }
        public string Status { get; set; }
        public string ImageUrl { get; set; }
        public int? Area { get; set; }
        public int? BedroomCount { get; set; }
        public int? BathroomCount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
        public string ProvinceName { get; set; }

    }
}
