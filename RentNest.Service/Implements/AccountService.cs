using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RentNest.Core.Consts;
using RentNest.Core.Domains;
using RentNest.Core.DTO;
using RentNest.Infrastructure.DataAccess;
using RentNest.Service.Interfaces;
namespace RentNest.Service.Implements
{
    public class AccountService : IAccountService
    {
        public async Task<bool> Login(AccountLoginDto accountDto)
        {
            var account = await AccountDAO.Instance.GetAccountByEmailAsync(accountDto.Email);
            if (account == null)
            {
                return false;
            }
            // hash with SHA256
            string hashPassword;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(accountDto.Password));
                hashPassword = BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
            if (hashPassword.Equals(account!.Password))
            {
                return true;
            }
            return false;
        }
        public async Task<Account?> GetAccountByEmailAsync(string email) => await AccountDAO.Instance.GetAccountByEmailAsync(email);
        public void Update(Account account) => AccountDAO.Instance.Update(account);
        public async Task<Account> CreateExternalAccountAsync(ExternalAccountRegisterDto dto)
        {
            var account = new Account
            {
                Email = dto.Email,
                AuthProvider = dto.AuthProvider.ToLower(),
                AuthProviderId = dto.AuthProviderId,
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
                throw new Exception("Lỗi khi tạo tài khoản: " + ex.Message);
            }
            return account;
        }


    }
}
