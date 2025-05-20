using RentNest.Core.Domains;
using RentNest.Core.DTO;

namespace RentNest.Service.Interfaces
{
    public interface IAccountService
    {
        Task<bool> Login(AccountLoginDto accountDto);
        Task<Account?> GetAccountByEmailAsync(string email);
        Task Update(Account account);
        Task<Account> CreateExternalAccountAsync(ExternalAccountRegisterDto dto);

    }

}
