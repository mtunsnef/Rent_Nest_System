using RentNest.Core.Domains;
using RentNest.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        Task AddMessage(Message message);
        Task<Message> GetMessageByConversationid(int id);
    }
}
