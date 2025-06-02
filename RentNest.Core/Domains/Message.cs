using System;
using System.Collections.Generic;

namespace RentNest.Core.Domains;

public partial class Message
{
    public int MessageId { get; set; }

    public int ConversationId { get; set; }

    public int SenderId { get; set; }

    public string? Content { get; set; }

    public string? ImageUrl { get; set; }

    public bool? IsRead { get; set; }

    public DateTime? SentAt { get; set; }

    public virtual Conversation Conversation { get; set; } = null!;

    public virtual Account Sender { get; set; } = null!;
}
