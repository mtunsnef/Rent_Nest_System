using Microsoft.AspNetCore.Http;
using RentNest.Core.Domains;
using RentNest.Core.DTO;

namespace RentNest.Service.Interfaces
{
    public interface IAccountService
    {
        Task<Boolean> Login(AccountLoginDto accountDto);
        Task<Account?> GetAccountByEmailAsync(string email);
        void Update(Account account);
        Task<Account> CreateGoogleAccountAsync(GoogleAccountRegisterDto dto);
        Task<UserProfile?> GetProfileAsync(int accountId);
        Task UpdateProfileAsync(UserProfile profile);
        Task<(bool Success, string Message)> UploadAvatarAsync(int accountId, IFormFile avatar, string webRootPath);
        Task AddUserProfile(UserProfile userProfile);



    }

}
