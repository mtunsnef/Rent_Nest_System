using System;
using System.Collections.Generic;

namespace RentNest.Core.Domains;

public partial class FavoritePost
{
    public int FavoriteId { get; set; }

    public int AccountId { get; set; }

    public int PostId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Post Post { get; set; } = null!;
}
