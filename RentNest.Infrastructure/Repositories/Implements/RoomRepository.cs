using RentNest.Core.Domains;
using RentNest.Infrastructure.DataAccess;
using RentNest.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.Implements
{
    public class RoomRepository : IRoomRepository
    {
        private readonly RoomDAO _roomDAO;

        public RoomRepository(RoomDAO roomDAO)
        {
            _roomDAO = roomDAO;
        }

        public List<Accommodation> GetAllRooms()
        {
            return _roomDAO.GetAllRooms();
        }

        public Accommodation? GetRoomById(int id)
        {
            return _roomDAO.GetRoomById(id);
        }
		public AccommodationDetail? GetRoomDetailById(int id)
        {
            return _roomDAO.GetRoomDetailById(id);
        }
        public int? GetDetailIdByAccommodationId(int accommodationId)
        {
            return _roomDAO.GetDetailIdByAccommodationId(accommodationId);
        }


    }

}
