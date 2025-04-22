using System;
using System.Collections.Generic;

namespace RentNest.Core.Domains;

public partial class LandLordVerification
{
    public int VerificationId { get; set; }

    public string IdCardNumber { get; set; } = null!;

    public string IdCardFrontUrl { get; set; } = null!;

    public string IdCardBackUrl { get; set; } = null!;

    public string? Status { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? VerifiedAt { get; set; }

    public string? RejectionReason { get; set; }

    public int? AccountId { get; set; }

    public int? VerifiedBy { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Account? VerifiedByNavigation { get; set; }
}
