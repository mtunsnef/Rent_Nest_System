using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Core.DTO
{
    public class GoogleAccountRegisterDto
    {
        public string Email { get; set; } = null!;
        public string GoogleId { get; set; } = null!;
        public string? Address { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn vai trò của bạn.")]
        public string Role { get; set; } = null!;
    }

}
