using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
using RentNest.Core.DTO;

namespace RentNest.Infrastructure.DataAccess
{
    public class RoomDAO : SingletonBase<RoomDAO>
    {
        private readonly RentNestSystemContext _context;
        public RoomDAO()
        {
            _context = new RentNestSystemContext();
        }
        public async Task<List<Accommodation>> GetRoomsBySearchDto(
    string provinceName,
    string districtName,
    string wardName,
    double? area,
    decimal? minMoney,
    decimal? maxMoney)
        {
            var query = _context.Accommodations.AsQueryable();

            if (!string.IsNullOrEmpty(provinceName))
            {
                query = query.Where(r => r.ProvinceName.Contains(provinceName));
            }

            if (!string.IsNullOrEmpty(districtName))
            {
                query = query.Where(r => r.DistrictName.Contains(districtName));
            }

            if (!string.IsNullOrEmpty(wardName))
            {
                query = query.Where(r => r.WardName.Contains(wardName));
            }

            if (area.HasValue)
            {
                query = query.Where(r => r.Area >= area.Value);
            }

            if (minMoney.HasValue)
            {
                query = query.Where(r => r.Price >= minMoney.Value);
            }

            if (maxMoney.HasValue)
            {
                query = query.Where(r => r.Price <= maxMoney.Value);
            }

            return await query.ToListAsync();
        }



        public async Task<string> GetRoomImage(int accommodationId)
        {
            var roomImages = await _context.AccommodationImages.Where(i => i.AccommodationId == accommodationId).ToListAsync();
            return roomImages!.Select(i => i.ImageUrl).FirstOrDefault()!;
        }
        public async Task<string> GetRoomType(int accommodationId)
        {
            var roomType = await _context.AccommodationTypes.Where(t => t.TypeId == accommodationId).FirstOrDefaultAsync();
            return roomType!.TypeName;
        }
    }
}