using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.Interfaces
{
    public interface IAccommodationRepository
    {
        Task<List<Post>> GetAccommodationsBySearchDto(
            string provinceName,
            string districtName,
            string wardName,
            double? area,
            decimal? minMoney,
            decimal? maxMoney);
        Task<string> GetAccommodationImage(int accommodationId);
        Task<string> GetAccommodationType(int accommodationId);
    }
}
