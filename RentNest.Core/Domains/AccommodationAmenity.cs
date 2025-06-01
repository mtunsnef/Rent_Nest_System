using System;
using System.Collections.Generic;

namespace RentNest.Core.Domains;

public partial class AccommodationAmenity
{
    public int Id { get; set; }

    public int AccommodationId { get; set; }

    public int AmenityId { get; set; }

    public virtual Accommodation Accommodation { get; set; } = null!;

    public virtual Amenity Amenity { get; set; } = null!;
}
