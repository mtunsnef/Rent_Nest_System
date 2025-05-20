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
        Task<bool> Login(AccountLoginDto accountDto);
        Task<Account?> GetAccountByEmailAsync(string email);
        Task Update(Account account);
        Task<Account> CreateExternalAccountAsync(ExternalAccountRegisterDto dto);
    }
}
