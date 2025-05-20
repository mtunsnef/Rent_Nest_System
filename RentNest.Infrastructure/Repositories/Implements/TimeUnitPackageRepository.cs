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
    public class TimeUnitPackageRepository : ITimeUnitPackageRepository
    {
        private readonly TimeUnitPackageDAO _timeUnitPackageDAO;

        public TimeUnitPackageRepository(TimeUnitPackageDAO timeUnitPackageDAO)
        {
            _timeUnitPackageDAO = timeUnitPackageDAO;
        }
        public async Task<List<TimeUnitPackage>> GetAll()
        {
            return await _timeUnitPackageDAO.GetAllAsync();
        }
    }
}
