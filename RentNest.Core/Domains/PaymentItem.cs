using System;
using System.Collections.Generic;

namespace RentNest.Core.Domains;

public partial class PaymentItem
{
    public int ItemId { get; set; }

    public string ItemType { get; set; } = null!;

    public decimal? Amount { get; set; }

    public int? PaymentId { get; set; }

    public virtual Payment? Payment { get; set; }
}
