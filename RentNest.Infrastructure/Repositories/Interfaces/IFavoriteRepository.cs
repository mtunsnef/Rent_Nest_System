using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.Interfaces
{
	public interface IFavoriteRepository
	{
		void Add(FavoritePost favoritePost);
        bool IsFavorite(int postId, int accountId);
        void RemoveFromFavorite(int postId, int accountId);
		List<FavoritePost> GetFavoriteByUser(int accountId);

	}
}
