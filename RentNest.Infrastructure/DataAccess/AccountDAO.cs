using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
namespace RentNest.Infrastructure.DataAccess
{
    public class AccountDAO : BaseDAO<Account>
    {
        public AccountDAO(RentNestSystemContext context) : base(context) { }

        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(a => a.Email == email);
        }
    }
}