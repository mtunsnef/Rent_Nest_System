using System;
using System.Collections.Generic;

namespace RentNest.Core.Domains;

public partial class PromoCode
{
    public int PromoId { get; set; }

    public string Code { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? DiscountPercent { get; set; }

    public decimal? DiscountAmount { get; set; }

    public int? DurationDays { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool? IsNewUserOnly { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<PromoUsage> PromoUsages { get; set; } = new List<PromoUsage>();
}
