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
        public async Task<List<Post>> GetAllPostsWithAccommodation()
        {
            return await _postRepository.GetAllPostsWithAccommodation();
        }
		public async Task<int?> GetAccommodationIdByPostId(int postId)
        {
            return await _postRepository.GetAccommodationIdByPostId(postId);
        }

	}
}
