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

    }

}
