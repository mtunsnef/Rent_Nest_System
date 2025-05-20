using RentNest.Core.Domains;
using RentNest.Infrastructure.Repositories.Interfaces;
using RentNest.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Implements
{
    public class TimeUnitPackageService : ITimeUnitPackageService
    {

        private readonly ITimeUnitPackageRepository _timeUnitPackageRepository;

        public TimeUnitPackageService(ITimeUnitPackageRepository timeUnitPackageRepository)
        {
            _timeUnitPackageRepository = timeUnitPackageRepository;
        }
        public async Task<IEnumerable<TimeUnitPackage>> GetAll()
        {
            return await _timeUnitPackageRepository.GetAll();
        }
    }
}
