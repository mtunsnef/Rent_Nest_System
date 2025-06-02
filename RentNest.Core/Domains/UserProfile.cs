using System;
using System.Collections.Generic;

namespace RentNest.Core.Domains;

public partial class UserProfile
{
    public int ProfileId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? AvatarUrl { get; set; }

    public string? Occupation { get; set; }

    public string? Address { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? AccountId { get; set; }

    public virtual Account? Account { get; set; }
}
