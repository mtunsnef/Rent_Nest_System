using RentNest.Core.Domains;
using RentNest.Core.DTO;
using RentNest.Infrastructure.DataAccess;
using RentNest.Infrastructure.Repositories.Interfaces;

namespace RentNest.Infrastructure.Repositories.Implements
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountDAO _accountDAO;
        private readonly UserProfileDAO _userProfileDAO;
        public AccountRepository(AccountDAO accountDAO, UserProfileDAO userProfileDAO)
        {
            _accountDAO = accountDAO;
            _userProfileDAO = userProfileDAO;
        }
        public async Task<bool> Login(AccountLoginDto accountDto)
        {
            var account = await _accountDAO.GetAccountByEmailAsync(accountDto.Email);
            if (account == null)
                return false;

            return BCrypt.Net.BCrypt.Verify(accountDto.Password, account.Password);
        }

        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            return await _accountDAO.GetAccountByEmailAsync(email);
        }

        public async Task Update(Account account)
        {
            await _accountDAO.UpdateAsync(account);
        }

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
                await _accountDAO.AddAsync(account);

                var userProfile = new UserProfile
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Address = dto.Address,
                    CreatedAt = DateTime.UtcNow,
                    AccountId = account.AccountId
                };

                await _userProfileDAO.AddAsync(userProfile);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tạo tài khoản: " + ex.Message);
            }
            return account;
        }
    }
}