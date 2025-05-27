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
	public class FavoriteRepository : IFavoriteRepository
	{
		private readonly FavoriteDAO _favoriteDAO;
		public FavoriteRepository(FavoriteDAO favoriteDAO)
		{
			_favoriteDAO = favoriteDAO;
		}
		public void Add(FavoritePost post)
		{
			_favoriteDAO.Add(post);
		}
        public bool IsFavorite(int postId, int accountId)
        {
            return _favoriteDAO.IsFavorite(postId, accountId);
        }

        public void RemoveFromFavorite(int postId, int accountId)
        {
            _favoriteDAO.RemoveFromFavorite(postId, accountId);
        }
		public List<FavoritePost> GetFavoriteByUser(int accountId)
		{
			return _favoriteDAO.GetFavoriteByUser(accountId);
		}
	}
}
