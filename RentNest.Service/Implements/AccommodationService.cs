using RentNest.Infrastructure.Repositories.Interfaces;
using RentNest.Core.Domains;
using RentNest.Infrastructure.DataAccess;
using RentNest.Service.Interfaces;

namespace RentNest.Service.Implements
{
    public class AccommodationService : IAccommodationService
    {
        private readonly IAccommodationRepository _accommodationRepository;

        public AccommodationService(IAccommodationRepository accommodationRepository)
        {
            _accommodationRepository = accommodationRepository;
        }
        public async Task<List<Accommodation>> GetAccommodationsBySearchDto(string provinceName,
            string districtName,
            string wardName,
            double? area,
            decimal? minMoney,
            decimal? maxMoney)
        {
            return await _accommodationRepository.GetAccommodationsBySearchDto(provinceName, districtName, wardName, area, minMoney, maxMoney);
        }
        public async Task<string> GetAccommodationImage(int accommodationId)
        {
            return await _accommodationRepository.GetAccommodationImage(accommodationId);
        }
        public async Task<string> GetAccommodationType(int accommodationId)
        {
            return await _accommodationRepository.GetAccommodationType(accommodationId);
        }
    }
}
