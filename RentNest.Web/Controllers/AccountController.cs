using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentNest.Core.UtilHelper;
using RentNest.Core.Consts;
using RentNest.Core.DTO;
using RentNest.Service.Implements;
using RentNest.Service.Interfaces;
using RentNest.Web.Models;

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
                Subject = "Reset Password - BlueTeam",
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

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            int? accountIdNullable = HttpContext.Session.GetInt32("AccountId");
            if (accountIdNullable == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin người dùng.";
                return RedirectToAction("Login", "Auth");
            }
            int accountId = accountIdNullable.Value;

            var profile = await _accountService.GetProfileAsync(accountId);
            var model = new ProfileViewModel
            {
                ProfileId = profile.ProfileId,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Gender = profile.Gender,
                DateOfBirth = profile.DateOfBirth,
                Address = profile.Address,
                AvatarUrl = profile.AvatarUrl,
                AccountId = accountId,
                Username = profile.Account?.Username,
                Email = profile.Account?.Email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ!";
                return View("Profile", model);
            }

            try
            {
                var userProfile = new UserProfile
                {
                    ProfileId = model.ProfileId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Gender = model.Gender,
                    DateOfBirth = model.DateOfBirth,
                    Address = model.Address,
                    AvatarUrl = model.AvatarUrl,
                    AccountId = model.AccountId,
                    UpdatedAt = DateTime.Now
                };

                await _accountService.UpdateProfileAsync(userProfile);
                TempData["SuccessMessage"] = "Thông tin cá nhân đã được cập nhật.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi cập nhật thông tin.";
                // Optional: log ex.Message to understand what failed
            }

            return RedirectToAction("Profile");
        }


        [HttpPost]
        public async Task<IActionResult> UploadAvatar(IFormFile avatar)
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId == null)
            {
                TempData["ErrorMessage"] = "Bạn chưa đăng nhập.";
                return RedirectToAction("Login", "Auth");
            }

            var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var (success, message) = await _accountService.UploadAvatarAsync(accountId.Value, avatar, webRootPath);

            if (success)
                TempData["SuccessMessage"] = message;
            else
                TempData["ErrorMessage"] = message;

            return RedirectToAction("Profile", "Account");
        }


    }
}
