﻿using System;
using System.Collections.Generic;

namespace RentNest.Core.Domains;

public partial class Post
{
    public int PostId { get; set; }

    public string CurrentStatus { get; set; } = null!;

    public int? ViewCount { get; set; }

    public DateTime? PublishedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int AccommodationId { get; set; }

    public int AccountId { get; set; }

    public virtual Accommodation Accommodation { get; set; } = null!;

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<PostApproval> PostApprovals { get; set; } = new List<PostApproval>();

    public virtual ICollection<PostPackageDetail> PostPackageDetails { get; set; } = new List<PostPackageDetail>();
}
