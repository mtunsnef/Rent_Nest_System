using RentNest.Core.Domains;
using RentNest.Core.DTO;
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
    }
}
