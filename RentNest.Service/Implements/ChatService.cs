using RentNest.Core.Domains;
using RentNest.Infrastructure.Repositories.Interfaces;
using RentNest.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Implements
{
    public class ChatService : IChatService
    {
        private readonly IMessageRepository _messageRepository;
        public ChatService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task AddMessage(Message message)
        {
            await _messageRepository.AddMessage(message);
        }

        public async Task<Message> GetMessageByConversationid(int id)
        {
            return await _messageRepository.GetMessageByConversationid(id);
        }
    }
}
