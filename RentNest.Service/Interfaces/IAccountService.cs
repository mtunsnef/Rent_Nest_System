using Microsoft.AspNetCore.Http;
using RentNest.Core.Domains;
using RentNest.Core.DTO;

namespace RentNest.Service.Interfaces
{
    public interface IAccountService
    {
        Task<bool> Login(AccountLoginDto accountDto);
        Task<Account?> GetAccountByEmailAsync(string email);
        Task Update(Account account);
        Task<UserProfile?> GetProfileAsync(int accountId);
        Task UpdateProfileAsync(UserProfile profile);
        Task<(bool Success, string Message)> UploadAvatarAsync(int accountId, IFormFile avatar, string webRootPath);
        Task<Account> CreateExternalAccountAsync(ExternalAccountRegisterDto dto);
        Task<bool> RegisterAccountAsync(AccountRegisterDto model);
        Task SetUserOnlineAsync(int userId, bool isOnline);
        Task UpdateLastActiveAsync(int userId);
    }

}
