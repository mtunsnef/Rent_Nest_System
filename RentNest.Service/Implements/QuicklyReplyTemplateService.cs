using RentNest.Core.Domains;
using RentNest.Core.DTO;
using RentNest.Infrastructure.Repositories.Interfaces;
using RentNest.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Implements
{
    public class QuicklyReplyTemplateService : IQuicklyReplyTemplateService
    {
        private readonly IQuickReplyTemplateRepository _quickReplyTemplateRepository;
        public QuicklyReplyTemplateService(IQuickReplyTemplateRepository quickReplyTemplateRepository)
        {
            _quickReplyTemplateRepository = quickReplyTemplateRepository;
        }

        public async Task AddQuickReplyAsync(string content, string targetRole, int accountId)
        {
            await _quickReplyTemplateRepository.AddQuickReplyAsync(content, targetRole, accountId);
        }

        public async Task<List<QuickMessDto>> GetQuickRepliesByRoleAsync(string role)
        {
            return await _quickReplyTemplateRepository.GetQuickRepliesByRoleAsync(role);
        }

    }
}
