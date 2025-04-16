using System.ComponentModel.DataAnnotations;

namespace RentNest.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string AccountEmail { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string AccountPassword { get; set; }
    }

}
