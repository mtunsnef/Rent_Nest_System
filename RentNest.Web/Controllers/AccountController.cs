using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentNest.Core.UtilHelper;
using RentNest.Core.Consts;
using RentNest.Core.DTO;
using RentNest.Service.Implements;
using RentNest.Service.Interfaces;

namespace RentNest.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IMailService _mailService;

        public AccountController(IAccountService accountService, IMailService mailService)
        {
            _accountService = accountService;
            _mailService = mailService;
        }
        [Authorize(AuthenticationSchemes = AuthSchemes.Cookie, Roles = $"{UserRoles.User},{UserRoles.Landlord}")]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail()
        {
            var account = await _accountService.GetAccountByEmailAsync(HttpContext.Session.GetString("Email")!);

            var updatePassword = "http://localhost:5216/Account/UpdatePassword";
            MailContent mail = new MailContent
            {
                To = account!.Email,
                Subject = "Reset Password - Furniture Shop",
                Body = "<h3>Click the link to reset your password:</h3>\n" +
               $"<a href='{updatePassword}'>Reset Password</a>\n" +
               $"<p>If you didn't request this, please ignore this email.</p>"
            };

            bool send = await _mailService.SendMail(mail);
            if (send)
            {
                TempData["SuccessMessage"] = "Mở hợp thư để đổi mật khẩu";
            }
            else
            {
                TempData["ErrorMessage"] = "Email gửi không thành công!";
            }

            return RedirectToAction("Index", "Home");
        }
        [Authorize(AuthenticationSchemes = AuthSchemes.Cookie, Roles = $"{UserRoles.User},{UserRoles.Landlord}")]
        [HttpGet]
        public IActionResult UpdatePassword()
        {
            return View();
        }
        [Authorize(AuthenticationSchemes = AuthSchemes.Cookie, Roles = $"{UserRoles.User},{UserRoles.Landlord}")]
        [HttpPost]
        public async Task<IActionResult> UpdatePassword([FromForm] ChangePasswordDto model)
        {
            var account = await _accountService.GetAccountByEmailAsync(HttpContext.Session.GetString("Email")!);
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (account != null)
            {
                if (!PasswordHelper.VerifyPassword(model.OldPassword!, account.Password!))
                {
                    TempData["ErrorMessage"] = "Mật Khẩu cũ không hợp lệ!";
                    return View(model);
                }
                else if (!string.Equals(model.NewPassword, model.ConfirmPassword))
                {
                    TempData["ErrorMessage"] = "Mật khẩu mới và xác nhận mật khẩu không trùng khớp!";
                    return View(model);
                }

                else if (model.OldPassword!.Equals(model.NewPassword))
                {
                    TempData["ErrorMessage"] = "Mật Khẩu mới và Mật Khẩu cũ trùng!";
                    return View(model);
                }
                else
                {

                    account.Password = PasswordHelper.HashPassword(model.NewPassword!);
                    await _accountService.Update(account);
                    TempData["SuccessMessage"] = "Đổi mật khẩu thành công! Đang chuyển hướng đến trang chủ...";
                    TempData["RedirectUrl"] = Url.Action("Index", "Home");
                }
            }
            return View();
        }

        // [HttpGet]
        // [Route("/Profile")]
        // public async Task<IActionResult> Profile()
        // {
        //     if (HttpContext.Session.GetString("AccountId") == null)
        //     {
        //         return RedirectToAction("Login", "Auth"); 
        //     }

        //     string email = HttpContext.Session.GetString("Email");
        //     var user = await _accountService.GetAccountByEmailAsync(email);
        //     return View(user);
        // }

        // [HttpPost]
        // public async Task<IActionResult> UpdateProfile(Account updatedAccount)
        // {
        //     if (HttpContext.Session.GetString("AccountId") == null)
        //     {
        //         return RedirectToAction("Login", "Auth");
        //     }

        //     string email = HttpContext.Session.GetString("Email");
        //     var user = await _accountService.GetAccountByEmailAsync(email);

        //     if (user != null)
        //     {
        //         user.AccountName = updatedAccount.AccountName;
        //         user.PhoneNumber = updatedAccount.PhoneNumber;
        //         user.Address = updatedAccount.Address;
        //         user.DateOfBirth = updatedAccount.DateOfBirth;
        //         user.Gender = updatedAccount.Gender;

        //         TempData["SuccessMessage"] = "Cập nhật thông tin cá nhân thành công!";
        //     }
        //     else
        //     {
        //         TempData["ErrorMessage"] = "Có lỗi xảy ra, vui lòng thử lại!";
        //     }
        //     return RedirectToAction("Profile");
        // }

    }
}
