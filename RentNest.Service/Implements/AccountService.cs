﻿using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RentNest.Core.UtilHelper;
using RentNest.Core.Consts;
using RentNest.Core.Domains;
using RentNest.Core.DTO;
using RentNest.Infrastructure.DataAccess;
using RentNest.Infrastructure.Repositories.Implements;
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
 
        public async Task<Account> CreateExternalAccountAsync(ExternalAccountRegisterDto dto)
        {
            return await _accountRepository.CreateExternalAccountAsync(dto);
        }

        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            return await _accountRepository.GetAccountByEmailAsync(email);
        }

        public async Task<bool> Login(AccountLoginDto accountDto)
        {
            return await _accountRepository.Login(accountDto);
        }

        public async Task Update(Account account)
        {
            await _accountRepository.Update(account);
        }

        public async Task<UserProfile?> GetProfileAsync(int accountId)
        {
            return await _accountRepository.GetProfileByAccountIdAsync(accountId);
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

        public async Task<bool> RegisterAccountAsync(AccountRegisterDto model)
        {
            return await _accountRepository.RegisterAccountAsync(model);
        }

        public async Task SetUserOnlineAsync(int userId, bool isOnline)
        {
            await _accountRepository.SetUserOnlineAsync(userId, isOnline);
        }

        public async Task UpdateLastActiveAsync(int userId)
        {
            await _accountRepository.UpdateLastActiveAsync(userId);
        }
    }
}
