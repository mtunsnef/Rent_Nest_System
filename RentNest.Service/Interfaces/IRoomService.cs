using RentNest.Core.Domains;
using RentNest.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentNest.Service.Interfaces
{
    public interface IRoomService
    {
        Task<List<Accommodation>> GetRoomsBySearchDto(string provinceName,
    string districtName,
    string wardName,
    double? area,
    decimal? minMoney,
    decimal? maxMoney);
        Task<string> GetRoomImage(int accommodationId);
        Task<string> GetRoomType(int accommodationId);
    }
}