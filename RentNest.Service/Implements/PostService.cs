using RentNest.Core.Domains;
using RentNest.Infrastructure.Repositories.Interfaces;
using RentNest.Service.Interfaces;

namespace RentNest.Service.Implements
{
    public class PostService : IPostService
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
