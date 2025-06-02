using RentNest.Core.Domains;
using RentNest.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Interfaces
{
        public interface IPostService
        {
            Task<List<Post>> GetAllPostsWithAccommodation();
            Task<Post?> GetPostDetailWithAccommodationDetailAsync(int postId);
            Task<List<Post>> GetAllPostsByUserAsync(int accountId);
            Task<int> SavePost(LandlordPostDto dto);
        }
}
