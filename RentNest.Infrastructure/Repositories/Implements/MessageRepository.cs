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
    public class MessageRepository : IMessageRepository
    {
        private readonly MessageDAO _messageDAO;
        public MessageRepository(MessageDAO messageDAO)
        {
            _messageDAO = messageDAO;
        }

        public async Task AddMessage(Message message)
        {
            await _messageDAO.AddAsync(message);
        }
        public async Task<Message> GetMessageByConversationid(int id)
        {
            return await _messageDAO.GetByIdAsync(id);
        }
    }
}
