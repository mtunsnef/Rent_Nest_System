using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.Interfaces
{
    public interface IConversationRepository
    {
        Task<IEnumerable<Conversation>> GetAll();
        Task<List<Conversation>> GetByUserIdAsync(int userId);
        Task<IEnumerable<object>> GetConversationMessagesAsync(int conversationId, int currentUserId);
        Task<Conversation?> GetConversationWithMessagesAsync(int conversationId);
        Task<Conversation> AddIfNotExistsAsync(int senderId, int receiverId, int? postId);
    }
}
