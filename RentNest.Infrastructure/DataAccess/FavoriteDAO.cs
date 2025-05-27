using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.DataAccess
{
	public class FavoriteDAO : BaseDAO<FavoriteDAO>
	{
		public FavoriteDAO(RentNestSystemContext context) : base(context) { }

		public void Add(FavoritePost favoritePost)
		{
			_context.FavoritePosts.Add(favoritePost);
			_context.SaveChanges();
		}
        public bool IsFavorite(int postId, int accountId)
        {
            return _context.FavoritePosts
                .Any(f => f.PostId == postId && f.AccountId == accountId);
        }

        public void RemoveFromFavorite(int postId, int accountId)
        {
            var favorite = _context.FavoritePosts
                .FirstOrDefault(f => f.PostId == postId && f.AccountId == accountId);

            if (favorite != null)
            {
                _context.FavoritePosts.Remove(favorite);
                _context.SaveChanges();
            }
        }
		public List<FavoritePost> GetFavoriteByUser(int accountId)
		{
			return _context.FavoritePosts
				.Where(f => f.AccountId == accountId)
				.Include(f => f.Post)
					.ThenInclude(p => p.Accommodation)
						.ThenInclude(a => a.AccommodationDetail)
				.Include(f => f.Post)
					.ThenInclude(p => p.Accommodation)
						.ThenInclude(a => a.AccommodationImages)
				.ToList();
		}

	}
}
