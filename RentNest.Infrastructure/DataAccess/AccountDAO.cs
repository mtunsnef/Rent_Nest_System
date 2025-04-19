using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
namespace RentNest.Infrastructure.DataAccess
{
    public class AccountDAO
    {
        private readonly RentNestSystemContext _context;
        public AccountDAO(RentNestSystemContext context)
        {
            _context = context;
        }
        public async Task<Account?> GetAccountByEmailAsync(string email) => await _context.Accounts.FirstOrDefaultAsync(account => account.Email.Equals(email));
    }
}