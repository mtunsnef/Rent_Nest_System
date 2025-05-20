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
    }
}
