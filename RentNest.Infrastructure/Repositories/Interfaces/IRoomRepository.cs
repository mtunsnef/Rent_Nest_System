using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        List<Accommodation> GetAllRooms();
        Accommodation? GetRoomById(int id);
        AccommodationDetail? GetRoomDetailById(int id);
        int? GetDetailIdByAccommodationId(int accommodationId);


    }
}
