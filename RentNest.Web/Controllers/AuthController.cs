using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using RentNest.Core.Consts;
using RentNest.Core.DTO;
using RentNest.Service.Interfaces;
using System.Security.Claims;
using RentNest.Core.UtilHelper;
using RentNest.Core.Domains;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using RentNest.Infrastructure.DataAccess;
using Humanizer;
namespace RentNest.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IMailService _mailService;
        private readonly IConfiguration _config;

        public AuthController(IAccountService accountService, IMailService mailService, IConfiguration config)
        {
            _accountService = accountService;
            _mailService = mailService;
            _config = config;
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
                TempData["ErrorMessage"] = "Email/Tên đăng nhập hoặc mật khẩu không hợp lệ!";
                return RedirectToAction("Login", "Auth");
            }

            var account = await _accountService.GetAccountByEmailOrUsernameAsync(model.EmailOrUsername);

            HttpContext.Session.SetInt32("AccountId", account.AccountId);
            HttpContext.Session.SetString("AccountName", account.Username!);
            HttpContext.Session.SetString("Email", account.Email);
            HttpContext.Session.SetString("LoginProvider", account.AuthProvider ?? "");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
                new Claim(ClaimTypes.Name, account.UserProfile?.FirstName ?? "" + " " + account.UserProfile?.LastName ?? ""),
                new Claim(ClaimTypes.Role, account.Role)
            };
            var identity = new ClaimsIdentity(claims, AuthSchemes.Cookie);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(AuthSchemes.Cookie, principal);

            TempData["SuccessMessage"] = "Đăng nhập thành công! Đang chuyển hướng đến trang chủ...";
            TempData["RedirectUrl"] = Url.Action("Index", "Home");

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
                TempData["AuthProviderId"] = externalId;
                TempData["AuthProvider"] = provider;

                return RedirectToAction("CompleteRegistration", "Auth");
            }

            HttpContext.Session.SetString("AccountId", account.AccountId.ToString());
            HttpContext.Session.SetString("Email", account.Email);
            HttpContext.Session.SetString("LoginProvider", account.AuthProvider ?? "");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
                new Claim(ClaimTypes.Name, name ?? ""),
                new Claim(ClaimTypes.Email, email ?? ""),
                new Claim(ClaimTypes.Role, account.Role)
            };

            var identity = new ClaimsIdentity(claims, scheme);
            await HttpContext.SignInAsync(AuthSchemes.Cookie, new ClaimsPrincipal(identity));

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
            var dto = new ExternalAccountRegisterDto
            {
                Email = TempData["Email"]?.ToString() ?? "",
                AuthProvider = TempData["AuthProvider"]?.ToString() ?? "",
                AuthProviderId = TempData["AuthProviderId"]?.ToString() ?? "",
                FirstName = TempData["FirstName"]?.ToString(),
                LastName = TempData["LastName"]?.ToString()
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> CompleteRegistration(ExternalAccountRegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var scheme = dto.AuthProvider.ToLower() switch
            {
                "google" => AuthSchemes.Google,
                "facebook" => AuthSchemes.Facebook,
                _ => throw new InvalidOperationException("Provider không hợp lệ.")
            };

            var authResult = await HttpContext.AuthenticateAsync(scheme);
            var externalPrincipal = authResult.Principal;

            if (externalPrincipal == null)
            {
                TempData["ErrorMessage"] = $"Không thể xác thực tài khoản {dto.AuthProvider}.";
                return RedirectToAction("Login", "Auth");
            }

            dto.AuthProviderId = externalPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

            var account = await _accountService.CreateExternalAccountAsync(dto);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
                new Claim(ClaimTypes.Name, $"{dto.FirstName} {dto.LastName}"),
                new Claim(ClaimTypes.GivenName, dto.FirstName ?? ""),
                new Claim(ClaimTypes.Surname, dto.LastName ?? ""),
                new Claim(ClaimTypes.Email, dto.Email),
                new Claim(ClaimTypes.Role, account.Role)
            };

            var identity = new ClaimsIdentity(claims, scheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(AuthSchemes.Cookie, principal);

            HttpContext.Session.SetInt32("AccountId", account.AccountId);
            HttpContext.Session.SetString("AccountName", $"{dto.FirstName} {dto.LastName}");
            HttpContext.Session.SetString("Email", dto.Email);
            HttpContext.Session.SetString("LoginProvider", account.AuthProvider ?? "");

            TempData["SuccessRegisMessage"] = "Đăng ký tài khoản thành công! Đang chuyển hướng bạn đến trang chủ...";
            TempData["RedirectUrl"] = Url.Action("Index", "Home");

            var baseUrl = _config["AppSettings:BaseUrl"];
            var mail = new MailContent
            {
                To = dto.Email,
                Subject = "Chào mừng đến với BlueHouse!",
                Body = $@"
                    <div style='font-family:Arial,sans-serif;padding:20px;color:#333'>
                        <h2>Chào mừng {dto.FirstName} {dto.LastName} đến với <span style='color:#007bff;'>BlueHouse</span>!</h2>
                        <p>Cảm ơn bạn đã đăng ký tài khoản bằng {dto.AuthProvider}. Chúng tôi rất vui khi có bạn là một phần của cộng đồng.</p>
                        <img src='https://localhost:7046/images/welcome-mail.jpg' alt='Welcome' style='max-width:100%;border-radius:10px;margin:20px 0' />
                        <p>Bắt đầu hành trình của bạn ngay hôm nay bằng cách khám phá các tính năng tuyệt vời của BlueHouse.</p>
                        <a href='{baseUrl}' style='display:inline-block;padding:10px 20px;background:#4b69bd;color:#fff;text-decoration:none;border-radius:5px;'>Truy cập BlueHouse</a>
                    </div>"
            };

            await _mailService.SendMail(mail);

            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(AccountRegisterDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("Mật khẩu", "Xác nhận mật khẩu không trùng.");
                return View(model);
            }

            if (await _accountService.CheckEmailExistsAsync(model.Email))
            {
                ModelState.AddModelError("Email", "Email đã được sử dụng. Vui lòng chọn email khác.");
                return View(model);
            }

            if (await _accountService.CheckUsernameExistsAsync(model.Username))
            {
                ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.");
                return View(model);
            }
            var result = await _accountService.RegisterAccountAsync(model);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi tạo tài khoản.");
                return View(model);
            }

            var account = await _accountService.GetAccountByEmailAsync(model.Email);

            HttpContext.Session.SetInt32("AccountId", account.AccountId);
            HttpContext.Session.SetString("AccountName", model.Username);
            HttpContext.Session.SetString("Email", model.Email);
            HttpContext.Session.SetString("LoginProvider", account.AuthProvider ?? "");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
                new Claim(ClaimTypes.Email, account.Email ?? ""),
                new Claim(ClaimTypes.Role, account.Role)
            };

            var identity = new ClaimsIdentity(claims, AuthSchemes.Cookie);
            await HttpContext.SignInAsync(AuthSchemes.Cookie, new ClaimsPrincipal(identity));

            TempData["SuccessRegisterLocalMessage"] = "Tài khoản đã được tạo thành công! Đang chuyển hướng đến trang chủ ...";
            TempData["RedirectUrl"] = Url.Action("Index", "Home");

            var baseUrl = _config["AppSettings:BaseUrl"];
            var mail = new MailContent
            {
                To = model.Email,
                Subject = "Chào mừng đến với BlueHouse!",
                Body = $@"
                    <div style='font-family:Arial,sans-serif;padding:20px;color:#333'>
                        <h2>Chào mừng bạn đến với <span style='color:#007bff;'>BlueHouse</span>!</h2>
                        <p>Cảm ơn bạn đã đăng ký tài khoản của website BlueHouse. Chúng tôi rất vui khi có bạn là một phần của cộng đồng.</p>
                        <img src='https://localhost:7046/images/welcome-mail.jpg' alt='Welcome' style='max-width:100%;border-radius:10px;margin:20px 0' />
                        <p>Bắt đầu hành trình của bạn ngay hôm nay bằng cách khám phá các tính năng tuyệt vời của BlueHouse.</p>
                        <a href='{baseUrl}' style='display:inline-block;padding:10px 20px;background:#4b69bd;color:#fff;text-decoration:none;border-radius:5px;'>Truy cập BlueHouse</a>
                    </div>"
            };
            await _mailService.SendMail(mail);

            return RedirectToAction("SignUp", "Auth");
        }

    }

}
