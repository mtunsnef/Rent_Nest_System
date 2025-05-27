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

        public async Task<List<Post>> GetAllPostsWithAccommodation()
        {
            return await _context.Posts
                .Include(p => p.Accommodation)
                    .ThenInclude(a => a.AccommodationImages)
                .Include(p => p.Accommodation)
                    .ThenInclude(a => a.AccommodationDetail) 
                .Where(p => p.CurrentStatus == "A" && p.Accommodation.Status != "I")
                .OrderByDescending(p => p.PublishedAt)
                .ToListAsync();
        }

		public async Task<int?> GetAccommodationIdByPostId(int postId)
		{
			var post = await _context.Posts
				.Where(p => p.PostId == postId)
				.Select(p => p.AccommodationId)
				.FirstOrDefaultAsync();

			return post == 0 ? null : post; // Assuming 0 means not found
		}


	}
}
