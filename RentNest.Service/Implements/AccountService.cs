using RentNest.Core.Domains;
using RentNest.Core.DTO;
using RentNest.Infrastructure.Repositories.Implements;
using RentNest.Infrastructure.Repositories.Interfaces;
using RentNest.Service.Interfaces;
namespace RentNest.Service.Implements
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository iAccountRepository;

        public AccountService()
        {
            iAccountRepository = new AccountRepository();
        }

        public async Task<Account> CreateExternalAccountAsync(ExternalAccountRegisterDto dto)
        {
            return await iAccountRepository.CreateExternalAccountAsync(dto);
        }

        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            return await iAccountRepository.GetAccountByEmailAsync(email);
        }

        public async Task<bool> Login(AccountLoginDto accountDto)
        {
            return await iAccountRepository.Login(accountDto);
        }

        public async Task Update(Account account)
        {
            await iAccountRepository.Update(account);
        }
    }
}
