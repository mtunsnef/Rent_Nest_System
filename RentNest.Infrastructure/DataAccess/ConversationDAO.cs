using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.DataAccess
{
    public class ConversationDAO : BaseDAO<Conversation>
    {
        public ConversationDAO(RentNestSystemContext context) : base(context) { }

        public override async Task<List<Conversation>> GetAllAsync()
            => await _dbSet
                    .Include(m => m.Messages)
                    .Include(a => a.Post)
                    .Include(a => a.Receiver)
                        .ThenInclude(a => a.UserProfile)
                    .Include(a => a.Sender)
                        .ThenInclude(a => a.UserProfile)
                    .ToListAsync();

        public async Task<List<Conversation>> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                 .Where(c => (c.SenderId == userId) || (c.ReceiverId == userId && c.Messages.Any()))
                 .Include(c => c.Messages)
                 .Include(c => c.Post)
                 .Include(c => c.Receiver)
                     .ThenInclude(u => u.UserProfile)
                 .Include(c => c.Sender)
                     .ThenInclude(u => u.UserProfile)
                 .ToListAsync();
        }

        public async Task<Conversation?> GetConversationWithMessagesAsync(int conversationId)
        {
            return await _dbSet
                .Include(c => c.Messages)
                    .ThenInclude(m => m.Sender)
                        .ThenInclude(u => u.UserProfile)
                .Include(c => c.Receiver)
                    .ThenInclude(u => u.UserProfile)
                .Include(c => c.Sender)
                    .ThenInclude(u => u.UserProfile)
                .Include(c => c.Post)
                    .ThenInclude(a => a.Accommodation)
                        .ThenInclude(a => a.AccommodationImages)
                .FirstOrDefaultAsync(c => c.ConversationId == conversationId);
        }
        public async Task<Conversation?> GetExistingConversationAsync(int userAId, int userBId, int? postId)
        {
            return await _dbSet.FirstOrDefaultAsync(c =>
                ((c.SenderId == userAId && c.ReceiverId == userBId) ||
                 (c.SenderId == userBId && c.ReceiverId == userAId))
                && c.PostId == postId
            );
        }

    }
}
