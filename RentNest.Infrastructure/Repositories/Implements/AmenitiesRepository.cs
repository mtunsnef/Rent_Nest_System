using RentNest.Core.Domains;
using RentNest.Infrastructure.DataAccess;
using RentNest.Infrastructure.Repositories.Interfaces;

namespace RentNest.Infrastructure.Repositories.Implements
{
    public class AmenitiesRepository : IAmenitiesRepository 
    {
        private readonly AmenitiesDAO _amenitiesDAO;

        public AmenitiesRepository(AmenitiesDAO amenitiesDAO)
        {
            _amenitiesDAO = amenitiesDAO;
        }
        public async Task<IEnumerable<Amenity>> GetAll()
        {
            return await _amenitiesDAO.GetAllAsync();
        }
    }
}
