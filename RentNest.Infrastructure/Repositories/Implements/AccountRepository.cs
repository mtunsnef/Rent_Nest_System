using RentNest.Core.Domains;
using RentNest.Infrastructure.DataAccess;
using RentNest.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.Implements
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountDAO _accountDAO;
        public AccountRepository(AccountDAO accountDAO)
        {
            _accountDAO = accountDAO;
        }
        public async Task<UserProfile?> GetProfileByAccountIdAsync(int accountId)
        {
            return await _accountDAO.GetProfileByAccountIdAsync(accountId);
        }
        public async Task UpdateProfileAsync(UserProfile profile)
        {
            await _accountDAO.UpdateProfileAsync(profile);
        }
        public async Task UpdateAvatarAsync(UserProfile profile, string avatarUrl)
        {
            profile.AvatarUrl = avatarUrl;
            await _accountDAO.UpdateProfileAsync(profile);
        }
        public async Task AddUserProfile(UserProfile userProfile)
        {
            await _accountDAO.AddUserProfile(userProfile);
        }

    }
}
