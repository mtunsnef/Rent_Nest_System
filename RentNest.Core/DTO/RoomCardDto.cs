using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentNest.Core.DTO
{
    public class RoomCardDto
    {
        public string? RoomTitle { get; set; }
        public string? RoomImage { get; set; }
        public decimal? RoomPrice { get; set; }
        public string? roomType { get; set; }
        public int? RoomArea { get; set; }
        public string? RoomAddress { get; set; }
        public string? RoomStatus { get; set; }
        public string? badgeClass { get; set; }

    }
}