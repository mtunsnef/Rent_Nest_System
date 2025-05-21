using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
using RentNest.Core.DTO;

namespace RentNest.Infrastructure.DataAccess
{
    public class PackagePricingDAO : BaseDAO<PackagePricing>
    {
        public PackagePricingDAO(RentNestSystemContext context) : base(context) { }
        public async Task<List<PackagePricing>> GetAllPackageOptions()
        {
            return await _dbSet
                .Include(t => t.TimeUnit)
                .Include(p => p.PackageType)
                .OrderBy(t => t.TimeUnit)
                .ThenBy(p => p.PackageType.Priority)
                .ToListAsync();
        }

        public async Task<List<PostPackageType>> GetPackageTypesByTimeUnit(int timeUnitId)
        {
            return await _dbSet
                .Where(p => p.TimeUnitId == timeUnitId)
                .Select(p => p.PackageType)
                .Distinct()
                .OrderBy(pt => pt.Priority)
                .ToListAsync();
        }

        public async Task<decimal?> GetUnitPrice(int timeUnitId, int packageTypeId)
        {
            return await _dbSet
                .Where(p => p.TimeUnitId == timeUnitId && p.PackageTypeId == packageTypeId)
                .Select(p => p.UnitPrice)
                .FirstOrDefaultAsync();
        }
        public async Task<List<PackagePricing>> GetPackagePricingsByTimeUnitAndType(int timeUnitId, int packageTypeId)
        {
            return await _dbSet
                .Include(p => p.TimeUnit)
                .Where(p => p.TimeUnitId == timeUnitId && p.PackageTypeId == packageTypeId)
                .OrderBy(p => p.DurationValue)
                .ToListAsync();
        }

        public async Task<int?> GetPricingIdAsync(int timeUnitId, int packageTypeId, int durationValue)
        {
            var pricing = await _dbSet
                .Where(p => p.TimeUnitId == timeUnitId
                            && p.PackageTypeId == packageTypeId
                            && p.DurationValue == durationValue
                            && p.IsActive == true)
                .Select(p => p.PricingId)
                .FirstOrDefaultAsync();

            return pricing == 0 ? null : pricing;
        }

    }
}
