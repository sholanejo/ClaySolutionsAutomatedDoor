using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ClaySolutionsAutomatedDoor.Infrastructure.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly AutomatedDoorDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public BaseRepository(AutomatedDoorDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(_dbContext));
            _dbSet = _dbContext.Set<TEntity>();
        }


        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Func<TEntity, bool> predicate)
        {
            return _dbSet.Where(predicate).AsEnumerable();
        }

        public async Task<TEntity> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(object id)
        {
            TEntity entityToDelete = await _dbSet.FindAsync(id);
            if (entityToDelete != null)
            {
                _dbSet.Remove(entityToDelete);
            }
        }
        public async Task InsertRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public Task<IQueryable<TEntity>> GetQueryAsync(Func<TEntity, bool> predicate)
        {
            return Task.FromResult(_dbSet.Where(predicate).AsQueryable());
        }

        public IQueryable<TEntity> GetAllQuery()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                _dbSet.Attach(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
            }

            await Task.CompletedTask;
        }
        public async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
            await Task.CompletedTask;
        }
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }
    }
}
