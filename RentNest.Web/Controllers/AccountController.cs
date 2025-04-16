using Microsoft.AspNetCore.Mvc;
using RentNest.Core.Domains;
using RentNest.Service.Interfaces;

namespace RentNest.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [Route("/Profile")]
        public async Task<IActionResult> Profile()
        {
            if (HttpContext.Session.GetString("AccountId") == null)
            {
                return RedirectToAction("Login", "Auth"); 
            }

            string email = HttpContext.Session.GetString("Email");
            var user = await _accountService.GetAccountByEmailAsync(email);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(Account updatedAccount)
        {
            if (HttpContext.Session.GetString("AccountId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            string email = HttpContext.Session.GetString("Email");
            var user = await _accountService.GetAccountByEmailAsync(email);

            if (user != null)
            {
                user.AccountName = updatedAccount.AccountName;
                user.PhoneNumber = updatedAccount.PhoneNumber;
                user.Address = updatedAccount.Address;
                user.DateOfBirth = updatedAccount.DateOfBirth;
                user.Gender = updatedAccount.Gender;

                TempData["SuccessMessage"] = "Cập nhật thông tin cá nhân thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra, vui lòng thử lại!";
            }
            return RedirectToAction("Profile");
        }

    }
}
