using RentNest.Core.Domains;
using RentNest.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Interfaces
{
    public interface IQuicklyReplyTemplateService
    {
        Task<List<QuickMessDto>> GetQuickRepliesByRoleAsync(string role);
        Task AddQuickReplyAsync(string content, string targetRole, int accountId);
    }
}
