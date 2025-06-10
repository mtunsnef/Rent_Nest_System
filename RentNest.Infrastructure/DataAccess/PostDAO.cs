using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RentNest.Core.Domains;
using RentNest.Core.UtilHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.DataAccess
{
    public class PostDAO : BaseDAO<Post>
    {
        public PostDAO(RentNestSystemContext context) : base(context) { }

        public async Task<List<Post>> GetAllPostsWithAccommodation()
        {
            var posts = await _dbSet.AsNoTracking()
                .Include(p => p.Accommodation)
                    .ThenInclude(a => a.AccommodationImages)
                .Include(p => p.Accommodation)
                    .ThenInclude(a => a.AccommodationDetail)
                .Include(p => p.PostPackageDetails)
                    .ThenInclude(p => p.Pricing)
                        .ThenInclude(t => t.PackageType)
                .Include(p => p.PostPackageDetails)
                    .ThenInclude(d => d.Pricing)
                        .ThenInclude(p => p.TimeUnit)
                .Include(p => p.Account)
                    .ThenInclude(u => u.UserProfile)
                .Where(p => p.CurrentStatus == "A" && p.Accommodation.Status != "I")
                .ToListAsync();

            var sortedPosts = posts
                .OrderByDescending(p =>
                {
                    var latestPackageName = p.PostPackageDetails
                        .OrderByDescending(ppd => ppd.CreatedAt)
                        .Select(ppd => ppd.Pricing.PackageType.PackageTypeName)
                        .FirstOrDefault();

                    var packageTypeEnum = BadgeHelper.ParsePackageType(latestPackageName ?? string.Empty);

                    return packageTypeEnum;
                })
                .ThenByDescending(p => p.PublishedAt)
                .ToList();

            return sortedPosts;
        }
        public async Task<List<Post>> GetTopVipPostsAsync()
        {
            var vipPosts = await _dbSet
                .Include(p => p.Accommodation)
                    .ThenInclude(a => a.AccommodationImages)
                .Include(p => p.Accommodation)
                    .ThenInclude(a => a.AccommodationDetail)
                .Include(p => p.PostPackageDetails)
                    .ThenInclude(ppd => ppd.Pricing)
                        .ThenInclude(pr => pr.PackageType)
                .Include(p => p.PostPackageDetails)
                    .ThenInclude(ppd => ppd.Pricing)
                        .ThenInclude(pr => pr.TimeUnit)
                .Include(p => p.Account)
                    .ThenInclude(acc => acc.UserProfile)
                .Where(p => p.CurrentStatus == "A" && p.Accommodation.Status != "I")
                .ToListAsync();

            var filtered = vipPosts
                .Select(p => new
                {
                    Post = p,
                    LatestPackage = p.PostPackageDetails
                        .OrderByDescending(ppd => ppd.CreatedAt)
                        .FirstOrDefault()
                })
                .Where(x =>
                    x.LatestPackage != null &&
                    (x.LatestPackage.Pricing.PackageType.PackageTypeName == "VIP Kim Cương" ||
                     x.LatestPackage.Pricing.PackageType.PackageTypeName == "VIP Vàng"))
                .OrderBy(x =>
                {
                    var package = x.LatestPackage.Pricing.PackageType.PackageTypeName;
                    return package == "VIP Kim Cương" ? 0 : 1;
                })
                .ThenByDescending(x => x.Post.PublishedAt)
                .Take(6)
                .Select(x => x.Post)
                .ToList();

            return filtered;
        }


        public async Task<Post?> GetPostDetailWithAccommodationDetailAsync(int postId)
        {
            return await _dbSet.AsNoTracking()
                .Include(p => p.Accommodation)
                    .ThenInclude(a => a.AccommodationDetail)
                .Include(p => p.Accommodation.AccommodationImages)
                .Include(p => p.PostPackageDetails)
                    .ThenInclude(p => p.Pricing)
                        .ThenInclude(t => t.PackageType)
                .Include(p => p.PostPackageDetails)
                    .ThenInclude(d => d.Pricing)
                        .ThenInclude(p => p.TimeUnit)
                .Include(a => a.Account)
                    .ThenInclude(u => u.UserProfile)
                .Include(a => a.Accommodation.AccommodationAmenities)
                    .ThenInclude(aa => aa.Amenity)
                .FirstOrDefaultAsync(p => p.PostId == postId);
        }

        public async Task<List<Post>> GetAllPostsByUserAsync(int accountId)
        {
            return await _context.Posts
                .Include(p => p.Accommodation)
                    .ThenInclude(a => a.AccommodationImages)
                .Include(p => p.Accommodation)
                    .ThenInclude(a => a.AccommodationDetail)
                .Include(p => p.PostPackageDetails)
                    .ThenInclude(d => d.Pricing)
                        .ThenInclude(p => p.PackageType)
                .Include(p => p.PostPackageDetails)
                    .ThenInclude(d => d.Pricing)
                        .ThenInclude(t => t.TimeUnit)
                .Include(p => p.PostApprovals)
                .Include(a => a.Account)
                    .ThenInclude(u => u.UserProfile)
                .Where(p => p.AccountId == accountId)
                .OrderByDescending(p => p.PublishedAt)
                .ToListAsync();
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
