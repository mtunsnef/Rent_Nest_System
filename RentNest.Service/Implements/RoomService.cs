using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RentNest.Core.Domains;
using RentNest.Core.DTO;
using RentNest.Infrastructure.DataAccess;
using RentNest.Service.Interfaces;

namespace RentNest.Service.Implements
{
    public class RoomService : IRoomService
    {
        public async Task<List<Accommodation>> GetRoomsBySearchDto(string provinceName,
    string districtName,
    string wardName,
    double? area,
    decimal? minMoney,
    decimal? maxMoney)
        {
            return await RoomDAO.Instance.GetRoomsBySearchDto(provinceName, districtName, wardName, area, minMoney, maxMoney);
        }
        public async Task<string> GetRoomImage(int accommodationId)
        {
            return await RoomDAO.Instance.GetRoomImage(accommodationId);
        }
        public async Task<string> GetRoomType(int accommodationId)
        {
            return await RoomDAO.Instance.GetRoomType(accommodationId);
        }
    }
}