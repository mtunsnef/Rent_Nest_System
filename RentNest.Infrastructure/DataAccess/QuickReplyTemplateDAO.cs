using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
using RentNest.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.DataAccess
{
    public class QuickReplyTemplateDAO : BaseDAO<QuickReplyTemplate>
    {
        public QuickReplyTemplateDAO(RentNestSystemContext context) : base(context) { }

        public async Task<List<QuickMessDto>> GetQuickRepliesByRoleAsync(string role)
        {
            return await _dbSet
                .Where(t => t.TargetRole.Equals(role))
                .OrderByDescending(c => c.CreatedAt) 
                .Select(t => new QuickMessDto
                {
                    Id = t.TemplateId,
                    Message = t.Message
                })
                .ToListAsync();
        }


    }
}
