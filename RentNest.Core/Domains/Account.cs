using System;
using System.Collections.Generic;

namespace RentNest.Core.Domains;

public partial class Account
{
    public int AccountId { get; set; }

    public string? Username { get; set; }

    public string Email { get; set; } = null!;

    public string? Password { get; set; }

    public string IsActive { get; set; } = null!;

    public string AuthProvider { get; set; } = null!;

    public string? AuthProviderId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string Role { get; set; } = null!;

    public virtual ICollection<Accommodation> Accommodations { get; set; } = new List<Accommodation>();

    public virtual LandLordVerification? LandLordVerificationAccount { get; set; }

    public virtual ICollection<LandLordVerification> LandLordVerificationVerifiedByNavigations { get; set; } = new List<LandLordVerification>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<PostApproval> PostApprovals { get; set; } = new List<PostApproval>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual UserProfile? UserProfile { get; set; }
}
