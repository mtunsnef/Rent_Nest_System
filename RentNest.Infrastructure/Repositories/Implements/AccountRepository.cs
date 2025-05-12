using RentNest.Core.Domains;
using RentNest.Core.DTO;
using RentNest.Infrastructure.DataAccess;
using RentNest.Infrastructure.Repositories.Interfaces;

namespace RentNest.Infrastructure.Repositories.Implements
{
    public class AccountRepository : IAccountRepository
    {
        public async Task<bool> Login(AccountLoginDto accountDto)
        {
            var account = await AccountDAO.Instance.GetAccountByEmailAsync(accountDto.Email);
            if (account == null)
            {
                return false;
            }
            var checkLogin = BCrypt.Net.BCrypt.Verify(accountDto.Password, account.Password);
            return checkLogin;
        }
        public async Task<Account?> GetAccountByEmailAsync(string email) => await AccountDAO.Instance.GetAccountByEmailAsync(email);
        public async Task Update(Account account) => await AccountDAO.Instance.Update(account);
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
