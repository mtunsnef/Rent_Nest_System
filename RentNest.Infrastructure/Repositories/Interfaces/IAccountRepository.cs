using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<UserProfile?> GetProfileByAccountIdAsync(int accountId);
        Task UpdateProfileAsync(UserProfile profile);
        Task UpdateAvatarAsync(UserProfile profile, string avatarUrl);
        Task AddUserProfile(UserProfile userProfile);

    }
}
