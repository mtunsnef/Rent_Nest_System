 using RentNest.Core.Domains;
using RentNest.Core.DTO;
using RentNest.Infrastructure.DataAccess;
using RentNest.Infrastructure.Repositories.Interfaces;

namespace RentNest.Infrastructure.Repositories.Implements
{
    public class PackagePricingRepository : IPackagePricingRepository
    {
        private readonly PackagePricingDAO _packagePricingDAO;
        private readonly TimeUnitPackageDAO _timeUnitPackageDAO;
        public PackagePricingRepository(PackagePricingDAO packagePricingDAO, TimeUnitPackageDAO timeUnitPackageDAO)
        {
            _packagePricingDAO = packagePricingDAO;
            _timeUnitPackageDAO = timeUnitPackageDAO;
        }

        public async Task<IEnumerable<PackagePricing>> GetAllPackageOptions()
        {
            return await _packagePricingDAO.GetAllPackageOptions();
        }

        public async Task<List<DurationPriceDto>> GetDurationsAndPrices(int timeUnitId, int packageTypeId)
        {
            var pricings = await _packagePricingDAO.GetPackagePricingsByTimeUnitAndType(timeUnitId, packageTypeId);

            return pricings.Select(p => new DurationPriceDto
            {
                DurationValue = p.DurationValue,
                UnitPrice = p.UnitPrice,
                TotalPrice = p.TotalPrice,
                TimeUnitName = p.TimeUnit?.TimeUnitName
            }).ToList();
        }

        public async Task<List<PackageTypeDto>> GetPackageTypes(int timeUnitId)
        {
            var packageTypes = await _packagePricingDAO.GetPackageTypesByTimeUnit(timeUnitId);
            var timeUnit = await _timeUnitPackageDAO.GetTimeUnitById(timeUnitId);

            var result = new List<PackageTypeDto>();

            foreach (var pt in packageTypes)
            {
                var unitPrice = await _packagePricingDAO.GetUnitPrice(timeUnitId, pt.PackageTypeId);

                result.Add(new PackageTypeDto
                {
                    PackageTypeId = pt.PackageTypeId,
                    PackageTypeName = pt.PackageTypeName,
                    Description = pt.Description,
                    TimeUnitName = timeUnit?.TimeUnitName,
                    UnitPrice = unitPrice ?? 0
                });
            }

            return result;
        }

        public async Task<List<PostPackageType>> GetPackageTypesByTimeUnit(int timeUnitId)
        {
            return await _packagePricingDAO.GetPackageTypesByTimeUnit(timeUnitId);
        }

        public async Task<int?> GetPricingIdAsync(int timeUnitId, int packageTypeId, int durationValue)
        {
            return await _packagePricingDAO.GetPricingIdAsync(timeUnitId, packageTypeId, durationValue);
        }
    }
}
