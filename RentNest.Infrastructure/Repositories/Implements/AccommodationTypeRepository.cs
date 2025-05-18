using RentNest.Core.Domains;
using RentNest.Infrastructure.DataAccess;
using RentNest.Infrastructure.Repositories.Interfaces;

namespace RentNest.Infrastructure.Repositories.Implements
{
    public class AccommodationTypeRepository : IAccommodationTypeRepository
    {
        private readonly AccommodationTypeDAO _accommodationTypeDAO;

        public AccommodationTypeRepository(AccommodationTypeDAO accommodationTypeDAO)
        {
            _accommodationTypeDAO = accommodationTypeDAO;
        }
        public async Task<IEnumerable<AccommodationType>> GetAllAsync()
        {
            return await _accommodationTypeDAO.GetAllAsync();
        }
    }
}
