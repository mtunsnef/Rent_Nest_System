using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using RentNest.Core.Consts;
using RentNest.Core.DTO;
using RentNest.Service.Interfaces;
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
        public async Task<IActionResult> Login([FromForm] AccountLoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (!await _accountService.Login(model))
            {
                TempData["ErrorMessage"] = "Email hoặc mật khẩu không hợp lệ!";
                return RedirectToAction("Login", "Auth");
            }
            var account = await _accountService.GetAccountByEmailAsync(model.Email);
            HttpContext.Session.SetString("AccountId", account!.AccountId.ToString());
            HttpContext.Session.SetString("AccountName", account.Username!);
            HttpContext.Session.SetString("Email", account.Email);
            // set authen 
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.Email),
                new Claim(ClaimTypes.Role, account.Role)
            };
            var identity = new ClaimsIdentity(claims, AuthSchemes.Cookie);
            var principal = new ClaimsPrincipal(identity);

            HttpContext.SignInAsync(AuthSchemes.Cookie, principal).Wait();
            TempData["SuccessMessage"] = "Đăng nhập thành công! Đang chuyển hướng đến trang chủ...";
            TempData["RedirectUrl"] = Url.Action("Index", "Home");


            // if (response.Content.RoleName == AppRoles.Admin.ToString())
            // {
            //     return RedirectToAction("Index", "Admin");
            // }
            return RedirectToAction("Login", "Auth");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(AuthSchemes.Cookie);
            HttpContext.Session.Clear();
            ModelState.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult ExternalLogin(string provider)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Auth", new { provider }, Request.Scheme);
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };

            return Challenge(properties, provider switch
            {
                "Google" => AuthSchemes.Google,
                "Facebook" => AuthSchemes.Facebook,
                _ => throw new ArgumentException("Provider không hợp lệ", nameof(provider))
            });
        }



        public async Task<IActionResult> ExternalLoginCallback(string provider)
        {
            var scheme = provider.ToLower() switch
            {
                AuthProviders.Google => AuthSchemes.Google,
                AuthProviders.Facebook => AuthSchemes.Facebook,
                _ => throw new NotSupportedException("Provider không hợp lệ.")
            };

            var result = await HttpContext.AuthenticateAsync(scheme);
            if (!result.Succeeded)
            {
                TempData["ErrorMessage"] = "Xác thực không thành công.";
                return RedirectToAction("Login", "Auth");
            }

            var principal = result.Principal;

            var externalId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
            var name = principal.FindFirst(ClaimTypes.Name)?.Value;
            var firstName = principal.FindFirst(ClaimTypes.GivenName)?.Value;
            var lastName = principal.FindFirst(ClaimTypes.Surname)?.Value;

            if (string.IsNullOrEmpty(externalId) || string.IsNullOrEmpty(email))
            {
                TempData["ErrorMessage"] = "Tài khoản email không hợp lệ.";
                return RedirectToAction("Login", "Auth");
            }

            var account = await _accountService.GetAccountByEmailAsync(email);
            if (account == null)
            {
                TempData["Email"] = email;
                TempData["FirstName"] = firstName ?? name;
                TempData["LastName"] = lastName ?? "";
                TempData["ExternalId"] = externalId;
                TempData["Provider"] = provider;

                return RedirectToAction("CompleteRegistration", "Auth");
            }

            HttpContext.Session.SetString("AccountId", account.AccountId.ToString());
            HttpContext.Session.SetString("Email", account.Email);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, name ?? ""),
                new Claim(ClaimTypes.Email, email ?? ""),
                new Claim(ClaimTypes.Role, account.Role)
            };

            var identity = new ClaimsIdentity(claims, scheme);
            var principalUser = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(AuthSchemes.Cookie, principalUser);

            TempData["SuccessMessage"] = "Đăng nhập thành công!";
            TempData["RedirectUrl"] = Url.Action("Index", "Home");

            return RedirectToAction("Login", "Auth");
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CompleteRegistration()
        {
            // Lấy TempData vào model
            var dto = new GoogleAccountRegisterDto
            {
                Email = TempData["Email"]?.ToString()!,
                GoogleId = TempData["GoogleId"]?.ToString()!,
                FirstName = TempData["FirstName"]?.ToString(),
                LastName = TempData["LastName"]?.ToString()
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> CompleteRegistration(GoogleAccountRegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var account = await _accountService.CreateGoogleAccountAsync(dto);

            var authenticateResult = await HttpContext.AuthenticateAsync(AuthSchemes.Google);
            var externalPrincipal = authenticateResult.Principal;

            if (externalPrincipal == null)
            {
                TempData["ErrorMessage"] = "Không thể xác thực tài khoản Google.";
                return RedirectToAction("Login", "Auth");
            }

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, dto.GoogleId),
                    new Claim(ClaimTypes.Name, $"{dto.FirstName} {dto.LastName}"),
                    new Claim(ClaimTypes.GivenName, dto.FirstName ?? ""),
                    new Claim(ClaimTypes.Surname, dto.LastName ?? ""),
                    new Claim(ClaimTypes.Email, dto.Email),
                };

            var identity = new ClaimsIdentity(claims, AuthSchemes.Google);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(AuthSchemes.Cookie, principal);

            HttpContext.Session.SetString("AccountId", account.AccountId.ToString());
            HttpContext.Session.SetString("AccountName", $"{dto.FirstName} {dto.LastName}");
            HttpContext.Session.SetString("Email", dto.Email);

            TempData["SuccessMessage"] = "Đăng ký tài khoản thành công!";
            TempData["RedirectUrl"] = Url.Action("Index", "Home");

            return RedirectToAction("Login", "Auth");
        }


    }

}
