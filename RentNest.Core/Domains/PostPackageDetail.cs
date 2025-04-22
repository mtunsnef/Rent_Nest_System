using System;
using System.Collections.Generic;

namespace RentNest.Core.Domains;

public partial class PostPackageDetail
{
    public int Id { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public decimal? ActualPrice { get; set; }

    public int PostId { get; set; }

    public int PostPackageId { get; set; }

    public virtual Post Post { get; set; } = null!;

    public virtual PostPackage PostPackage { get; set; } = null!;
}
