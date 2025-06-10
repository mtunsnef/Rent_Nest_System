using RentNest.Core.Domains;
using RentNest.Core.DTO;

namespace RentNest.Service.Interfaces
{
    public interface IPostService
    {
        Task<List<Post>> GetAllPostsWithAccommodation();
        Task<Post?> GetPostDetailWithAccommodationDetailAsync(int postId);
        Task<List<Post>> GetAllPostsByUserAsync(int accountId);
        Task<int> SavePost(LandlordPostDto dto);
        Task<List<Post>> GetTopVipPostsAsync();
    }
}
