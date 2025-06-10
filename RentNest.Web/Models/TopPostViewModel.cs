namespace RentNest.Web.Models
{
    public class TopPostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public decimal? Price { get; set; }
        public string AvatarUrl { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
        public string ProvinceName { get; set; }
        public string PackageTypeName { get; set; }
        public string TimeUnitName { get; set; }
    }
}
