using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.DataAccess
{
    public class UserProfileDAO : SingletonBase<UserProfileDAO>
    {
        private readonly RentNestSystemContext _context;
        public UserProfileDAO()
        {
            _context = new RentNestSystemContext();
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
