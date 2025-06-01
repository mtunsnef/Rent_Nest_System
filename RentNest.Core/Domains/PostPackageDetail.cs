using System;
using System.Collections.Generic;

namespace RentNest.Core.Domains;

public partial class PostPackageDetail
{
    public int Id { get; set; }

    public int PostId { get; set; }

    public int PricingId { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string? PaymentStatus { get; set; }

    public string? PaymentTransactionId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Post Post { get; set; } = null!;

    public virtual PackagePricing Pricing { get; set; } = null!;
}
