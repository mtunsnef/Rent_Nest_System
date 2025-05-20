using System;
using System.Collections.Generic;

namespace RentNest.Core.Domains;

public partial class Accommodation
{
    public int AccommodationId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string Address { get; set; } = null!;

    public decimal? Price { get; set; }

    public decimal? DepositAmount { get; set; }

    public int? Area { get; set; }

    public int? MaxOccupancy { get; set; }

    public string? VideoUrl { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int OwnerId { get; set; }

    public int TypeId { get; set; }

    public string? WardName { get; set; }

    public string? DistrictName { get; set; }

    public string? ProvinceName { get; set; }

    public virtual AccommodationDetail? AccommodationDetail { get; set; }

    public virtual ICollection<AccommodationImage> AccommodationImages { get; set; } = new List<AccommodationImage>();

    public virtual Account Owner { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual AccommodationType Type { get; set; } = null!;
}
