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
            if (string.IsNullOrEmpty(account.Email))
            {
                throw new ArgumentException("Email cannot be empty.");
            }

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
        }

        public async Task<UserProfile?> GetProfileByAccountIdAsync(int accountId)
        {
           
            return await _context.UserProfiles
                .Include(p => p.Account)
                .FirstOrDefaultAsync(p => p.AccountId == accountId);
        }

        public async Task UpdateProfileAsync(UserProfile updatedProfile)
        {
            var existingProfile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.ProfileId == updatedProfile.ProfileId);

            if (existingProfile == null)
            {
                throw new Exception("Profile not found.");
            }

            // Manually update fields
            existingProfile.FirstName = updatedProfile.FirstName;
            existingProfile.LastName = updatedProfile.LastName;
            existingProfile.Gender = updatedProfile.Gender;
            existingProfile.DateOfBirth = updatedProfile.DateOfBirth;
            existingProfile.Address = updatedProfile.Address;
            existingProfile.Occupation = updatedProfile.Occupation;
            existingProfile.AvatarUrl = updatedProfile.AvatarUrl;
            existingProfile.UpdatedAt = DateTime.Now;

            // Save changes
            await _context.SaveChangesAsync();
        }

        public async Task AddUserProfile(UserProfile userProfile)
        {
            try
            {
                _context.UserProfiles.Add(userProfile);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm: " + ex.Message);
            }
        }

    }
}