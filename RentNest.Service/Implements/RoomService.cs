using RentNest.Infrastructure.Repositories.Interfaces;
using System.Collections.Generic;
using System.Text;
using System;
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
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public List<Accommodation> GetAllRooms()
        {
            return _roomRepository.GetAllRooms();
        }

        public Accommodation? GetRoomById(int id)
        {
            return _roomRepository.GetRoomById(id);
        }
        public AccommodationDetail? GetRoomDetailById(int id)
        {
            return _roomRepository.GetRoomDetailById(id);
        }
        public int? GetDetailIdByAccommodationId(int accommodationId)
        {
            return _roomRepository.GetDetailIdByAccommodationId(accommodationId);
        }

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
