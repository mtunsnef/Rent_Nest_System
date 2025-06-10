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
using RentNest.Core.Domains;

namespace RentNest.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IMailService _mailService;
        private readonly IConfiguration _config;
        public AccountController(IAccountService accountService, IMailService mailService, IConfiguration config)
        {
            _accountService = accountService;
            _mailService = mailService;
            _config = config;
        }
        [Authorize(AuthenticationSchemes = AuthSchemes.Cookie, Roles = $"{UserRoles.User},{UserRoles.Landlord}")]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail()
        {
            var account = await _accountService.GetAccountByEmailAsync(HttpContext.Session.GetString("Email")!);

            var baseUrl = _config["AppSettings:BaseUrl"];
            var updatePassword = $"{baseUrl}/Account/UpdatePassword";

            MailContent mail = new MailContent
            {
                To = account!.Email,
                Subject = "BlueHouse - Liên kết cập nhật mật khẩu của bạn đã sẵn sàng",
                Body = $@"
                <div style='font-family: Arial, sans-serif;'>
                    <h3>Xin chào {account.UserProfile?.FirstName} {account.UserProfile?.LastName},</h3>
                    <p>Bạn vừa yêu cầu cập nhật mật khẩu. Vui lòng nhấn vào liên kết dưới đây để thực hiện:</p>
                    <a href='{updatePassword}' style='padding: 10px 10px; background-color: #4b69bd; color: white; text-decoration: underline; border-radius: 5px;'>Cập nhât mật khẩu</a>
                    <p>Nếu bạn không yêu cầu điều này, hãy bỏ qua email này.</p>
                    <br/>
                    <p>Trân trọng, BlueTeam</p>
                </div>"
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
                    TempData["ErrorMessage"] = "Mật khẩu cũ không hợp lệ!";
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
            int? accountIdNullable = User.GetUserId();
            if (accountIdNullable == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin người dùng.";
                return RedirectToAction("Login", "Auth");
            }
            int accountId = accountIdNullable.Value;

            var profile = await _accountService.GetProfileAsync(accountId);
            var model = new ProfileViewModel
            {
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Gender = profile.Gender,
                DateOfBirth = profile.DateOfBirth,
                Address = profile.Address,
                AvatarUrl = profile.AvatarUrl ?? "/images/default-avatar.jpg",
                AccountId = accountId,
                Username = profile.Account?.Username,
                Email = profile.Account?.Email,
                PhoneNumber = profile.PhoneNumber,
                Occupation = profile.Occupation
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
                    AccountId = model.AccountId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Gender = model.Gender,
                    DateOfBirth = model.DateOfBirth,
                    Address = model.Address,
                    Occupation = model.Occupation,
                    PhoneNumber = model.PhoneNumber,
                    AvatarUrl = model.AvatarUrl,
                    UpdatedAt = model.UpdatedAt
                };

                await _accountService.UpdateProfileAsync(userProfile);
                TempData["SuccessMessage"] = "Thông tin cá nhân đã được cập nhật.";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi cập nhật thông tin.";
            }

            return RedirectToAction("Profile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadAvatar(IFormFile avatar)
        {
            var accountId = User.GetUserId();
            if (accountId == null)
            {
                return Unauthorized(new { success = false, message = "Bạn chưa đăng nhập." });
            }

            var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var (success, message) = await _accountService.UploadAvatarAsync(accountId.Value, avatar, webRootPath);

            return Json(new { success, message });
        }
    }
}
