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
    public class AccommodationRepository : IAccommodationRepository
    {
        private readonly AccommodationDAO _accommodationDAO;

        public AccommodationRepository(AccommodationDAO accommodationDAO)
        {
            _accommodationDAO = accommodationDAO;
        }

        public Task<string> GetAccommodationImage(int accommodationId)
        {
            return _accommodationDAO.GetAccommodationImage(accommodationId);
        }

        public Task<string> GetAccommodationType(int accommodationId)
        {
            return _accommodationDAO.GetAccommodationType((int)accommodationId);
        }
        public Task<List<Accommodation>> GetAccommodationsBySearchDto(string provinceName, string districtName, string wardName, double? area, decimal? minMoney, decimal? maxMoney)
        {
            return _accommodationDAO.GetRoomsBySearchDto(provinceName, districtName, wardName, area, minMoney, maxMoney);
        }
    }
}
