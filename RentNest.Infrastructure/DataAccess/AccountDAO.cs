using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
namespace RentNest.Infrastructure.DataAccess
{
    public class AccountDAO : SingletonBase<AccountDAO>
    {
        private readonly RentNestSystemContext _context;
        public AccountDAO()
        {
            _context = new RentNestSystemContext();
        }
        public async Task<Account?> GetAccountByEmailAsync(string email) => await _context.Accounts.FirstOrDefaultAsync(account => account.Email.Equals(email));
        public void Update(Account account)
        {

            _context.Accounts.Attach(account);
            _context.Entry(account).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public async Task AddAccount(Account account)
        {
            try
            {
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm: " + ex.Message);
            }
        }
    }
}