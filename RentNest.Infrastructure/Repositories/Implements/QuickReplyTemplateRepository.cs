using RentNest.Core.Domains;
using RentNest.Core.DTO;
using RentNest.Infrastructure.DataAccess;
using RentNest.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.Implements
{
    public class QuickReplyTemplateRepository : IQuickReplyTemplateRepository
    {
        private readonly QuickReplyTemplateDAO _quickReplyTemplateDAO;

        public QuickReplyTemplateRepository(QuickReplyTemplateDAO quickReplyTemplateDAO)
        {
            _quickReplyTemplateDAO = quickReplyTemplateDAO;
        }

        public async Task AddQuickReplyAsync(string message, string targetRole, int accountId)
        {
            var quickMess = new QuickReplyTemplate
            {
                Message = message,
                TargetRole = targetRole,
                AccountId = accountId
            };
            await _quickReplyTemplateDAO.AddAsync(quickMess);
        }

        public async Task<List<QuickMessDto>> GetQuickRepliesByRoleAsync(string role)
        {
            return await _quickReplyTemplateDAO.GetQuickRepliesByRoleAsync(role);
        }

    }
}
