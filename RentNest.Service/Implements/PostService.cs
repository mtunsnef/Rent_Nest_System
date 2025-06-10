using RentNest.Core.Domains;
using RentNest.Core.DTO;
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

        public async Task<List<Post>> GetAllPostsByUserAsync(int accountId)
        {
            return await _postRepository.GetAllPostsByUserAsync(accountId);
        }

        public async Task<List<Post>> GetAllPostsWithAccommodation()
        {
            return await _postRepository.GetAllPostsWithAccommodation();
        }

        public async Task<Post?> GetPostDetailWithAccommodationDetailAsync(int postId)
        {
            return await _postRepository.GetPostDetailWithAccommodationDetailAsync(postId);
        }

        public async Task<List<Post>> GetTopVipPostsAsync()
        {
            return await _postRepository.GetTopVipPostsAsync();
        }

        public async Task<int> SavePost(LandlordPostDto dto)
        {
            return await _postRepository.SavePost(dto);
        }
    }
}
