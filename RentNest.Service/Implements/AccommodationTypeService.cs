using RentNest.Core.Domains;
using RentNest.Infrastructure.DataAccess;
using RentNest.Infrastructure.Repositories.Interfaces;
using RentNest.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Implements
{
    public class AccommodationTypeService : IAccommodationTypeService
    {
        private readonly IAccommodationTypeRepository _iAccommodationTypeRepository;

        public AccommodationTypeService(IAccommodationTypeRepository iAccommodationTypeRepository)
        {
            _iAccommodationTypeRepository = iAccommodationTypeRepository;
        }
        public async Task<IEnumerable<AccommodationType>> GetAllAsync()
        {
            return await _iAccommodationTypeRepository.GetAllAsync();
        }
    }
}
