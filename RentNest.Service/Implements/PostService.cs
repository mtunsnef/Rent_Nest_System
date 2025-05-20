using RentNest.Core.Domains;
using RentNest.Infrastructure.Repositories.Interfaces;
using RentNest.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Implements
{
    public class PostService:IPostService
    {
        private readonly IPostRepository _postRepository;
        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public List<Post> GetAllPostsWithAccommodation()
        {
            return _postRepository.GetAllPostsWithAccommodation();
        }
    }
}
