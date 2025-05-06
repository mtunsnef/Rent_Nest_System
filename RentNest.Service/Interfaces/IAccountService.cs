using RentNest.Core.Domains;
using RentNest.Core.DTO;

namespace RentNest.Service.Interfaces
{
    public interface IAccountService
    {
        Task<Boolean> Login(AccountLoginDto accountDto);
        Task<Account?> GetAccountByEmailAsync(string email);
        void Update(Account account);
        Task<Account> CreateExternalAccountAsync(ExternalAccountRegisterDto dto);

    }

}
