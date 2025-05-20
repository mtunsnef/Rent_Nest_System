using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RentNest.Common.UtilHelper;
using RentNest.Core.Consts;
using RentNest.Core.Domains;
using RentNest.Core.DTO;
using RentNest.Infrastructure.DataAccess;
using RentNest.Infrastructure.Repositories.Interfaces;
using RentNest.Service.Interfaces;
namespace RentNest.Service.Implements
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository) 
        {
            _accountRepository = accountRepository;
        }


        //public async Task<bool> Login(AccountLoginDto accountDto)
        //{
        //    var account = await AccountDAO.Instance.GetAccountByEmailAsync(accountDto.Email);
        //    if (account == null)
        //    {
        //        return false;
        //    }
        //    // hash with SHA256
        //    string hashPassword;
        //    using (SHA256 sha256 = SHA256.Create())
        //    {
        //        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(accountDto.Password));
        //        hashPassword = BitConverter.ToString(bytes).Replace("-", "").ToLower();
        //    }
        //    if (hashPassword.Equals(account!.Password))
        //    {
        //        return true;
        //    }
        //    return false;
        //}



        public async Task<bool> Login(AccountLoginDto accountDto)
        {
            var account = await AccountDAO.Instance.GetAccountByEmailAsync(accountDto.Email);
            if (account == null)
                return false;

            // Use helper instead of direct Bcrypt call
            if (PasswordHelper.VerifyPassword(accountDto.Password, account.Password))
            {
                return true;
            }
            return false;
        }



        public async Task<Account?> GetAccountByEmailAsync(string email) => await AccountDAO.Instance.GetAccountByEmailAsync(email);
        public void Update(Account account) => AccountDAO.Instance.Update(account);
        public async Task<Account> CreateGoogleAccountAsync(GoogleAccountRegisterDto dto)
        {
            var account = new Account
            {
                Email = dto.Email,
                AuthProvider = AuthProviders.Google,
                AuthProviderId = dto.GoogleId,
                Role = dto.Role,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await AccountDAO.Instance.AddAccount(account);

                var userProfile = new UserProfile
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Address = dto.Address,
                    CreatedAt = DateTime.UtcNow,
                    AccountId = account.AccountId
                };

                await UserProfileDAO.Instance.AddUserProfile(userProfile);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tạo tài khoản Google: " + ex.Message);
            }

            return account;
        }

        public async Task<UserProfile?> GetProfileAsync(int accountId)
        {
            return await _accountRepository.GetProfileByAccountIdAsync(accountId);
        }

        public async Task AddUserProfile(UserProfile userProfile)
        {
            await _accountRepository.AddUserProfile(userProfile);
        }


        public async Task UpdateProfileAsync(UserProfile profile)
        {
           
            await _accountRepository.UpdateProfileAsync(profile);
        }

        public async Task<(bool Success, string Message)> UploadAvatarAsync(int accountId, IFormFile avatar, string webRootPath)
        {
            if (avatar == null || avatar.Length == 0)
                return (false, "Vui lòng chọn một ảnh hợp lệ.");

            var profile = await _accountRepository.GetProfileByAccountIdAsync(accountId);
            if (profile == null)
                return (false, "Không tìm thấy hồ sơ người dùng.");

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(avatar.FileName)}";
            var filePath = Path.Combine(webRootPath, "images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await avatar.CopyToAsync(stream);
            }

            await _accountRepository.UpdateAvatarAsync(profile, $"/images/{fileName}");

            return (true, "Cập nhật ảnh đại diện thành công!");
        }

    }
}
