using System;
using System.Collections.Generic;

namespace RentNest.Core.Domains;

public partial class PackagePricing
{
    public int PricingId { get; set; }

    public int TimeUnitId { get; set; }

    public int PackageTypeId { get; set; }

    public int DurationValue { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual PostPackageType PackageType { get; set; } = null!;

    public virtual ICollection<PostPackageDetail> PostPackageDetails { get; set; } = new List<PostPackageDetail>();

    public virtual TimeUnitPackage TimeUnit { get; set; } = null!;
}
