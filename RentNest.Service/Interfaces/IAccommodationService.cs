using System.Text;
using RentNest.Core.Domains;
using RentNest.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentNest.Service.Interfaces
{
    public interface IAccommodationService
    {
        List<Accommodation> GetAllAccommodation();
        Accommodation? GetAccommodationById(int id);
        AccommodationDetail? GetAccommodationDetailById(int id);
        int? GetDetailIdByAccommodationId(int accommodationId);
        Task<List<Accommodation>> GetAccommodationsBySearchDto(
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