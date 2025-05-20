using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.DataAccess
{
    public class PostDAO: BaseDAO<PostDAO>
    {
        public PostDAO(RentNestSystemContext context) : base(context) { }

		public List<Post> GetAllPostsWithAccommodation()
        {
            return _context.Posts
        .Include(p => p.Accommodation)
            .ThenInclude(a => a.AccommodationImages)
        .Where(p => p.CurrentStatus == "A" && p.Accommodation.Status != "I")
        .OrderByDescending(p => p.PublishedAt) 
        .ToList();
        }
    }
}
