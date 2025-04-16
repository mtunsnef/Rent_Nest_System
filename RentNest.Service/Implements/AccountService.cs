using RentNest.Core.Domains;
using RentNest.Service.Interfaces;

namespace RentNest.Service.Implements
{
    public class AccountService : IAccountService
    {
        private readonly List<Account> _accounts = new()
        {
            new Account {
                AccountId = 1,
                AccountName = "Minh Tún",
                Email = "admin@gmail.com",
                PhoneNumber = "0941673660",
                Address = "Quảng Ninh, Quảng Bình",
                DateOfBirth = new DateTime(2003, 11, 23),
                Gender = "Nam",
                Password = "123123",
                AccountRole = 1
            },
            new Account {
                AccountId = 2,
                AccountName = "MinhTuns",
                Email = "user@gmail.com",
                PhoneNumber = "0941673660",
                Address = "Quảng Ninh, Quảng Bình",
                DateOfBirth = new DateTime(2003, 11, 23),
                Gender = "Nam",
                Password = "123123",
                AccountRole = 0
            },
            new Account {
                AccountId = 3,
                AccountName = null,
                Email = "tuannmde170204@fpt.edu.vn",
                PhoneNumber = "0941673660",
                Address = "Quảng Ninh, Quảng Bình",
                DateOfBirth = new DateTime(2003, 11, 23),
                Gender = "Nam",
                Password = null,
                AccountRole = 0
            }
        };


        public Task<Account?> GetAccountByEmailAsync(string email)
        {
            var user = _accounts.FirstOrDefault(u => u.Email == email);
            return Task.FromResult(user);
        }
    }

}
