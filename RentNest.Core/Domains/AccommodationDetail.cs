using System;
using System.Collections.Generic;

namespace RentNest.Core.Domains;

public partial class AccommodationDetail
{
    public int DetailId { get; set; }

    public bool? HasKitchenCabinet { get; set; }

    public bool? HasAirConditioner { get; set; }

    public bool? HasRefrigerator { get; set; }

    public bool? HasWashingMachine { get; set; }

    public bool? HasLoft { get; set; }

    public string? FurnitureStatus { get; set; }

    public int? BedroomCount { get; set; }

    public int? BathroomCount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int AccommodationId { get; set; }

    public virtual Accommodation Accommodation { get; set; } = null!;
}
