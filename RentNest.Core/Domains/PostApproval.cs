using System;
using System.Collections.Generic;

namespace RentNest.Core.Domains;

public partial class PostApproval
{
    public int ApprovalId { get; set; }

    public string Status { get; set; } = null!;

    public string? RejectionReason { get; set; }

    public string? Note { get; set; }

    public DateTime? ProcessedAt { get; set; }

    public int PostId { get; set; }

    public int? ApprovedByAccountId { get; set; }

    public virtual Account? ApprovedByAccount { get; set; }

    public virtual Post Post { get; set; } = null!;
}
