using System;
using System.Collections.Generic;

namespace RentNest.Core.Domains;

public partial class PostPackage
{
    public int PackageId { get; set; }

    public string PackageName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int? DurationDays { get; set; }

    public string? Image { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsFeatured { get; set; }

    public bool? IsPriority { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<PostPackageDetail> PostPackageDetails { get; set; } = new List<PostPackageDetail>();
}
