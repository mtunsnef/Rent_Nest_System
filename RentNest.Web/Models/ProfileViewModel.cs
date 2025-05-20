using System.ComponentModel.DataAnnotations;

namespace RentNest.Web.Models
{
    public class ProfileViewModel
    {
        public int ProfileId { get; set; }

        [Display(Name = "Họ")]
        public string? FirstName { get; set; }

        [Display(Name = "Tên")]
        public string? LastName { get; set; }

        [Display(Name = "Giới tính")]
        public string? Gender { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateOnly? DateOfBirth { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public string? AvatarUrl { get; set; }

        [Display(Name = "Nghề nghiệp")]
        public string? Occupation { get; set; }

        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }

        public int AccountId { get; set; }

        // Additional from Account entity
        [Display(Name = "Tên tài khoản")]
        public string? Username { get; set; }

        [Display(Name = "Email")]
        public string? Email { get; set; }
    }

}
