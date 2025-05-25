namespace RentNest.Web.Models
{
    public class ChatConversationViewModel
    {
        public int ConversationId { get; set; }
        public int ReceiverId { get; set; }
        public string ReceiverFullName { get; set; }
        public string? ReceiverAvatarUrl { get; set; }
        public DateTime? LastActiveAt { get; set; }
        public bool? IsOnline { get; set; }
        public int? PostId { get; set; }
        public string? PostTitle { get; set; }
        public string? PostImageUrl { get; set; }
        public decimal? PostPrice { get; set; }

        public List<MessageViewModel> Messages { get; set; } = new();
    }

    public class MessageViewModel
    {
        public int SenderId { get; set; }
        public string? ImageUrl { get; set; }
        public string Content { get; set; }
        public DateTime? SentAt { get; set; }
    }

}
