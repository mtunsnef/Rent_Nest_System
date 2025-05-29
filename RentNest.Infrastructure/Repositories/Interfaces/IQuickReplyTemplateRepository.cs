using RentNest.Core.Domains;
using RentNest.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.Interfaces
{
    public interface IQuickReplyTemplateRepository
    {
        Task<List<QuickMessDto>> GetQuickRepliesByRoleAsync(string role);
        Task AddQuickReplyAsync(string content, string targetRole, int accountId);
    }
}
