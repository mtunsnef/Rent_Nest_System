using Microsoft.AspNetCore.Http;
using RentNest.Core.Domains;
using RentNest.Core.DTO;

namespace RentNest.Service.Interfaces
{
    public interface IAccountService
    {
        Task<bool> Login(AccountLoginDto accountDto);
        Task<Account?> GetAccountByEmailAsync(string email);
        void Update(Account account);
        Task<UserProfile?> GetProfileAsync(int accountId);
        Task UpdateProfileAsync(UserProfile profile);
        Task<(bool Success, string Message)> UploadAvatarAsync(int accountId, IFormFile avatar, string webRootPath);
        Task AddUserProfile(UserProfile userProfile);
        Task<Account> CreateExternalAccountAsync(ExternalAccountRegisterDto dto);

    }

}
