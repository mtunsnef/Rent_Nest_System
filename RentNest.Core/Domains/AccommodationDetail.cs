using System;
using System.Collections.Generic;

namespace RentNest.Core.Domains;

public partial class AccommodationDetail
{
    public int DetailId { get; set; }

    public bool? HasKitchen { get; set; }

    public bool? HasAirConditioner { get; set; }

    public bool? HasParking { get; set; }

    public bool? HasSecurity { get; set; }

    public bool? HasElevator { get; set; }

    public string? FurnitureStatus { get; set; }

    public bool? IsPetsAllowed { get; set; }

    public bool? IsSmokingAllowed { get; set; }

    public bool? HasWashingMachine { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int AccommodationId { get; set; }

    public virtual Accommodation Accommodation { get; set; } = null!;
}
