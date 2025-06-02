using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
using RentNest.Infrastructure.DataAccess;
using RentNest.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.Implements
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly ConversationDAO _conversationDAO;
        public ConversationRepository(ConversationDAO conversationDAO)
        {
            _conversationDAO = conversationDAO;
        }

        public async Task<Conversation> AddIfNotExistsAsync(int senderId, int receiverId, int? postId)
        {
            var existing = await _conversationDAO.GetExistingConversationAsync(senderId, receiverId, postId);

            if (existing != null) return existing;

            var conversation = new Conversation
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                PostId = postId,
                StartedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            await _conversationDAO.AddAsync(conversation);
            return conversation;
        }

        public async Task<IEnumerable<Conversation>> GetAll()
        {
            return await _conversationDAO.GetAllAsync();
        }

        public async Task<List<Conversation>> GetByUserIdAsync(int userId)
        {
            return await _conversationDAO.GetByUserIdAsync(userId);
        }
        public async Task<IEnumerable<object>> GetConversationMessagesAsync(int conversationId, int currentUserId)
        {
            var conversation = await _conversationDAO.GetConversationWithMessagesAsync(conversationId);
            if (conversation == null) return Enumerable.Empty<object>();

            return conversation.Messages
                .OrderBy(m => m.SentAt)
                .Select(m => new
                {
                    SenderName = m.Sender.UserProfile.FirstName + " " + m.Sender.UserProfile.LastName,
                    SenderAvatar = m.Sender.UserProfile.AvatarUrl ?? "/images/person_1.jpg",
                    Content = m.Content,
                    Timestamp = m.SentAt?.ToString("HH:mm dd/MM/yyyy"),
                    IsMe = m.SenderId == currentUserId
                });
        }

        public async Task<Conversation?> GetConversationWithMessagesAsync(int conversationId)
        {
            return await _conversationDAO.GetConversationWithMessagesAsync(conversationId);
        }
    }
}
