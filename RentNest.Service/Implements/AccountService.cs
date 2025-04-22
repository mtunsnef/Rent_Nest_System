using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using RentNest.Core.Domains;
using RentNest.Core.DTO;
using RentNest.Infrastructure.DataAccess;
using RentNest.Service.Interfaces;
namespace RentNest.Service.Implements
{
    public class AccountService : IAccountService
    {
        private readonly AccountDAO _accountDAO;
        public AccountService(AccountDAO accountDAO)
        {
            _accountDAO = accountDAO;
        }
        public async Task<Boolean> Login(AccountLoginDto accountDto)
        {
            var account = await _accountDAO.GetAccountByEmailAsync(accountDto.Email);
            if (account == null)
            {
                return false;
            }
            // hash with SHA256
            string hashPassword;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(accountDto.Password));
                hashPassword = BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
            if (hashPassword.Equals(account!.Password))
            {
                return true;
            }
            return false;
        }
        public async Task<Account?> GetAccountByEmailAsync(string email) => await _accountDAO.GetAccountByEmailAsync(email);
        public void Update(Account account) => _accountDAO.Update(account);
    }
}
