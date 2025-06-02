using System;
using System.Collections.Generic;

namespace RentNest.Core.Domains;

public partial class QuickReplyTemplate
{
    public int TemplateId { get; set; }

    public string Message { get; set; } = null!;

    public bool? IsActive { get; set; }

    public bool? IsDefault { get; set; }

    public int? AccountId { get; set; }

    public string? TargetRole { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Account? Account { get; set; }
}
