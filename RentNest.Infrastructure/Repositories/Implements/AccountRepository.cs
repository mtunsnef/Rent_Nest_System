using RentNest.Core.Domains;
using RentNest.Infrastructure.DataAccess;
using RentNest.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentNest.Core.DTO;
using RentNest.Core.UtilHelper;

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

        public async Task<UserProfile?> GetProfileByAccountIdAsync(int accountId)
        {
            return await _accountDAO.GetProfileByAccountIdAsync(accountId);
        }

        public async Task UpdateProfileAsync(UserProfile profile)
        {
            await _accountDAO.UpdateProfileAsync(profile);
        }

        public async Task UpdateAvatarAsync(UserProfile profile, string avatarUrl)
        {
            profile.AvatarUrl = avatarUrl;
            await _accountDAO.UpdateProfileAsync(profile);
        }

        public async Task AddUserProfile(UserProfile userProfile)
        {
            await _accountDAO.AddUserProfile(userProfile);
        }
        
        public async Task<bool> Login(AccountLoginDto accountDto)
        {
            try
            {
                var user = await _accountDAO.GetAccountByEmailAsync(accountDto.Email);

                if (user == null || !PasswordHelper.VerifyPassword(accountDto.Password, user.Password))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi đăng nhập: " + ex.Message);
            }
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