using System;
using System.Collections.Generic;

namespace RentNest.Core.Domains;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int PostPackageDetailsId { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? Status { get; set; }

    public DateTime? PaymentDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? MethodId { get; set; }

    public int? AccountId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual PaymentMethod? Method { get; set; }

    public virtual PostPackageDetail PostPackageDetails { get; set; } = null!;
}
