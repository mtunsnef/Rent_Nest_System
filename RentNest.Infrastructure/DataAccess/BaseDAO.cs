using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.DataAccess
{
    public class BaseDAO<TEntity> where TEntity : class
    {
        protected readonly RentNestSystemContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseDAO(RentNestSystemContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public virtual async Task<TEntity?> GetByIdAsync(object id)
            => await _dbSet.FindAsync(id);

        public virtual async Task AddAsync(TEntity entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

}
