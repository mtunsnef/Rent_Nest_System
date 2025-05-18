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
    public class AmenitiesService : IAmenitiesSerivce
    {

        private readonly IAmenitiesRepository _amenitiesRepository;

        public AmenitiesService(IAmenitiesRepository amenitiesRepository)
        {
            _amenitiesRepository = amenitiesRepository;
        }
        public async Task<IEnumerable<Amenity>> GetAll()
        {
            return await _amenitiesRepository.GetAll();
        }
    }
}
