using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RentNest.Service.Interfaces;
using RentNest.Web.Models;
using System.Security.Claims;

namespace RentNest.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAccountService _accountService;

        public AuthController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _accountService.GetAccountByEmailAsync(model.AccountEmail);
            if (user == null || user.Password != model.AccountPassword)
            {
                TempData["ErrorMessage"] = "Email hoặc mật khẩu không hợp lệ!";
                return RedirectToAction("Login", "Auth");
            }

            HttpContext.Session.SetString("AccountId", user.AccountId.ToString());
            //HttpContext.Session.SetString("AccountName", user.AccountName);
            HttpContext.Session.SetString("Email", user.Email);
            HttpContext.Session.SetString("Role", user.AccountRole == 1 ? "Staff" : "Member");

            TempData["SuccessMessage"] = "Đăng nhập thành công! Đang chuyển hướng đến trang chủ...";
            TempData["RedirectUrl"] = Url.Action("Index", "Home");

            return RedirectToAction("Login", "Auth");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult ExternalLogin(string provider)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Auth", null, Request.Scheme);
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, provider);
        }

        //public async Task<IActionResult> ExternalLoginCallback()
        //{
        //    var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        //    var googleId = authenticateResult.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    var email = authenticateResult.Principal.FindFirst(ClaimTypes.Email)?.Value;
        //    var name = authenticateResult.Principal.FindFirst(ClaimTypes.Name)?.Value;

        //    if (string.IsNullOrEmpty(googleId) || string.IsNullOrEmpty(email))
        //    {
        //        TempData["ErrorMessage"] = "Tài khoản email này không hợp lệ!";
        //        return RedirectToAction("Login", "Auth");
        //    }

        //    var user = await _accountService.GetAccountByEmailAsync(email);
        //    if (user == null)
        //    {
        //        TempData["ErrorMessage"] = "Tài khoản không tồn tại trong hệ thống!";
        //        return RedirectToAction("Login", "Auth");
        //    }

        //    HttpContext.Session.SetString("AccountId", user.AccountId.ToString());
        //    //HttpContext.Session.SetString("AccountName", user.AccountName);
        //    HttpContext.Session.SetString("Email", user.Email);
        //    HttpContext.Session.SetString("Role", user.AccountRole == 1 ? "Staff" : "Member");

        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, name ?? ""),
        //        new Claim(ClaimTypes.Email, email ?? "")
        //    };

        //    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //    var principal = new ClaimsPrincipal(identity);

        //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        //    TempData["SuccessMessage"] = "Đăng nhập thành công! Đang chuyển hướng đến trang chủ...";
        //    TempData["RedirectUrl"] = Url.Action("Index", "Home");

        //    return RedirectToAction("Login", "Auth");
        //}

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }

}
