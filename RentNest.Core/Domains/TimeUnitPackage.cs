using System;
using System.Collections.Generic;

namespace RentNest.Core.Domains;

public partial class TimeUnitPackage
{
    public int TimeUnitId { get; set; }

    public string TimeUnitName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<PackagePricing> PackagePricings { get; set; } = new List<PackagePricing>();
}
