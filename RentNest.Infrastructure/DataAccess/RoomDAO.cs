using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.DataAccess
{
    public class RoomDAO:SingletonBase<RoomDAO>
    {
        private readonly RentNestSystemContext _context;
        public RoomDAO()
        {
            _context = new RentNestSystemContext();
        }
        public List<Accommodation> GetAllRooms()
        {
            return _context.Accommodations
                .Include(a => a.AccommodationImages)
                .Include(a => a.Type) // Type = AccommodationType
                .Where(a => a.Status != "I")
                .ToList();
        }

        public Accommodation? GetRoomById(int id)
        {
            return _context.Accommodations
                .Include(a => a.AccommodationImages)
                .Include(a => a.Type)
                .Include(a => a.AccommodationDetail)
                .FirstOrDefault(a => a.AccommodationId == id && a.Status != "I");
        }

		public AccommodationDetail? GetRoomDetailById(int id)
		{
			return _context.AccommodationDetails
				.Include(ad => ad.Accommodation)
				.ThenInclude(a => a.AccommodationImages)
				.FirstOrDefault(ad => ad.DetailId == id && ad.Accommodation.Status != "I");
		}

		public int? GetDetailIdByAccommodationId(int accommodationId)
        {
            return _context.AccommodationDetails
                .Where(ad => ad.AccommodationId == accommodationId && ad.Accommodation.Status != "I")
                .Select(ad => (int?)ad.DetailId)
                .FirstOrDefault();
        }


    }
}
