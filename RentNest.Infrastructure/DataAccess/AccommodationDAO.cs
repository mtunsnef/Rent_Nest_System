using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.DataAccess
{
    public class AccommodationDAO : BaseDAO<AccommodationDAO>
    {
        public AccommodationDAO(RentNestSystemContext context) : base(context) { }
        public List<Accommodation> GetAllAccommodation()
        {
            return _context.Accommodations
                .Include(a => a.AccommodationImages)
                .Include(a => a.Type) // Type = AccommodationType
                .Where(a => a.Status != "I")
                .ToList();
        }
        public Accommodation? GetAccommodationById(int id)
        {
            return _context.Accommodations
                .Include(a => a.AccommodationImages)
                .Include(a => a.Type)
                .Include(a => a.AccommodationDetail)
                .FirstOrDefault(a => a.AccommodationId == id && a.Status != "I");
        }

        public AccommodationDetail? GetAccommodationDetailById(int id)
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

        public async Task<string> GetAccommodationImage(int accommodationId)
        {
            var roomImages = await _context.AccommodationImages.Where(i => i.AccommodationId == accommodationId).ToListAsync();
            return roomImages!.Select(i => i.ImageUrl).FirstOrDefault()!;
        }
        public async Task<string> GetAccommodationType(int accommodationId)
        {
            var roomType = await _context.AccommodationTypes.Where(t => t.TypeId == accommodationId).FirstOrDefaultAsync();
            return roomType!.TypeName;
        }
    }
}
