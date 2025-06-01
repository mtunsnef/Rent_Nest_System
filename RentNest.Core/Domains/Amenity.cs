using System;
using System.Collections.Generic;

namespace RentNest.Core.Domains;

public partial class Amenity
{
    public int AmenityId { get; set; }

    public string AmenityName { get; set; } = null!;

    public string? IconSvg { get; set; }

    public virtual ICollection<AccommodationAmenity> AccommodationAmenities { get; set; } = new List<AccommodationAmenity>();
}
