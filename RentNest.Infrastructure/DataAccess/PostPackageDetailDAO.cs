using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.DataAccess
{
    public class PostPackageDetailDAO : BaseDAO<PostPackageDetail>
    {
        public PostPackageDetailDAO(RentNestSystemContext context) : base(context) { }
        public async Task<PostPackageDetail?> GetByPostIdAsync(int postId)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PostId == postId);
        }
    }
}
