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
using Microsoft.EntityFrameworkCore;

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
            return await _userProfileDAO.GetProfileByAccountIdAsync(accountId);
        }

        public async Task UpdateProfileAsync(UserProfile profile)
        {
            await _userProfileDAO.UpdateProfileAsync(profile);
        }

        public async Task UpdateAvatarAsync(UserProfile profile, string avatarUrl)
        {
            profile.AvatarUrl = avatarUrl;
            await _userProfileDAO.UpdateProfileAsync(profile);
        }

        public async Task<bool> Login(AccountLoginDto accountDto)
        {
            try
            {
                var user = await _accountDAO.GetAccountByEmailOrUsernameAsync(accountDto.EmailOrUsername);

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
                    PhoneNumber = dto.PhoneNumber,
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

        public async Task<bool> RegisterAccountAsync(AccountRegisterDto model)
        {
            var account = new Account
            {
                Username = model.Username,
                Email = model.Email,
                Password = PasswordHelper.HashPassword(model.Password),
                Role = model.Role,
                IsActive = "A",
                AuthProvider = "local",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _accountDAO.AddAsync(account);

            var profile = new UserProfile
            {
                AccountId = account.AccountId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _userProfileDAO.AddAsync(profile);
            return true;
        }

        public async Task SetUserOnlineAsync(int userId, bool isOnline)
        {
            var user = await _accountDAO.GetByIdAsync(userId);
            if (user != null)
            {
                user.IsOnline = isOnline;
                await _accountDAO.UpdateAsync(user);
            }
        }

        public async Task UpdateLastActiveAsync(int userId)
        {
            var user = await _accountDAO.GetByIdAsync(userId);
            if (user != null)
            {
                user.LastActiveAt = DateTime.UtcNow.AddHours(7);
                await _accountDAO.UpdateAsync(user);
            }
        }

        public async Task<Account> GetAccountById(int accountId)
        {
            return await _accountDAO.GetByIdAsync(accountId);
        }

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _accountDAO.CheckEmailExistsAsync(email);
        }

        public async Task<bool> CheckUsernameExistsAsync(string username)
        {
            return await _accountDAO.CheckEmailExistsAsync(username);
        }

        public async Task<Account?> GetAccountByEmailOrUsernameAsync(string input)
        {
            return await _accountDAO.GetAccountByEmailOrUsernameAsync(input);
        }
    }
}