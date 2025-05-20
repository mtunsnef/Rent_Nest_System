using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Core.DTO
{
    public class ExternalAccountRegisterDto
    {
        public string Email { get; set; } = null!;
        public string AuthProvider { get; set; } = null!;
        public string AuthProviderId { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ của bạn.")]
        public string? Address { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại của bạn.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn vai trò của bạn.")]
        public string Role { get; set; } = null!;
    }

}
