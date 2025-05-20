using System;
using System.Collections.Generic;

namespace RentNest.Core.Domains;

public partial class PromoUsage
{
    public int PromoUsageId { get; set; }

    public int AccountId { get; set; }

    public int PromoId { get; set; }

    public int PostId { get; set; }

    public DateTime? UsedAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Post Post { get; set; } = null!;

    public virtual PromoCode Promo { get; set; } = null!;
}
