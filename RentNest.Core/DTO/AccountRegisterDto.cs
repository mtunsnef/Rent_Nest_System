using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Core.DTO
{
    public class AccountRegisterDto
    {
        [Required(ErrorMessage = "Tên người dùng không được để trống")]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email không được để trống")]

        public string Email { get; set; }

        [MinLength(6, ErrorMessage = "Vui lòng nhập mật khẩu trên 6 ký tự")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; }

        [MinLength(6, ErrorMessage = "Vui lòng nhập mật khẩu trên 6 ký tự")]
        [Compare("Password", ErrorMessage = "Mật khẩu không trùng khớp")]
        [Required(ErrorMessage = "Xác nhận mật khẩu không được để trống")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn vai trò của bạn")]
        public string Role { get; set; } // "U" or "L"
    }

}
