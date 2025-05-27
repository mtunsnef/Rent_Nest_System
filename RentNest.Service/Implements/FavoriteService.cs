using RentNest.Core.Domains;
using RentNest.Infrastructure.Repositories.Implements;
using RentNest.Infrastructure.Repositories.Interfaces;
using RentNest.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Implements
{
	public class FavoriteService : IFavoriteService
	{
		private readonly IFavoriteRepository _favoriteRepo;

		public FavoriteService(IFavoriteRepository favoriteRepo)
		{
			_favoriteRepo = favoriteRepo;
		}

		public void AddToFavorite(int postId, int accountId)
		{
			_favoriteRepo.Add(new FavoritePost
			{
				PostId = postId,
				AccountId = accountId,
				CreatedAt = DateTime.Now
			});
		}
        public bool IsFavorite(int postId, int accountId)
        {
            return _favoriteRepo.IsFavorite(postId, accountId);
        }

        public void RemoveFromFavorite(int postId, int accountId)
        {
            _favoriteRepo.RemoveFromFavorite(postId, accountId);
        }
		public List<FavoritePost> GetFavoriteByUser(int accountId)
		{
			return _favoriteRepo.GetFavoriteByUser(accountId);
		}
	}
}
