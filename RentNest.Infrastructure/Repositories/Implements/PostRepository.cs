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
    public class PostRepository:IPostRepository
    {
        private readonly PostDAO _postDAO;

        public PostRepository(PostDAO postDAO)
        {
            _postDAO = postDAO;
        }
        public List<Post> GetAllPostsWithAccommodation()
        {
            return _postDAO.GetAllPostsWithAccommodation();
        }
    }
}
