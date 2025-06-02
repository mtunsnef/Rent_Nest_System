using RentNest.Core.Domains;
using RentNest.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<UserProfile?> GetProfileByAccountIdAsync(int accountId);
        Task UpdateProfileAsync(UserProfile profile);
        Task UpdateAvatarAsync(UserProfile profile, string avatarUrl);
        Task<bool> Login(AccountLoginDto accountDto);
        Task<Account?> GetAccountByEmailAsync(string email);
        Task Update(Account account);
        Task<Account> CreateExternalAccountAsync(ExternalAccountRegisterDto dto);
        Task<bool> RegisterAccountAsync(AccountRegisterDto model);
        Task SetUserOnlineAsync(int userId, bool isOnline);
        Task UpdateLastActiveAsync(int userId);
        Task<Account> GetAccountById(int accountId);
    }
}
