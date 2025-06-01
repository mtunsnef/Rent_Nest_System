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
    public class PostRepository : IPostRepository
    {
        private readonly PostDAO _postDAO;

        public PostRepository(PostDAO postDAO)
        {
            _postDAO = postDAO;
        }

        public async Task<List<Post>> GetAllPostsByUserAsync(int accountId)
        {
            return await _postDAO.GetAllPostsByUserAsync(accountId);
        }

        public async Task<List<Post>> GetAllPostsWithAccommodation()
        {
            return await _postDAO.GetAllPostsWithAccommodation();
        }

        public async Task<Post?> GetPostDetailWithAccommodationDetailAsync(int postId)
        {
            return await _postDAO.GetPostDetailWithAccommodationDetailAsync(postId);
        }
    }
}
