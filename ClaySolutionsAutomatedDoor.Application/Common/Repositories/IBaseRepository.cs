using System.Linq.Expressions;

namespace ClaySolutionsAutomatedDoor.Application.Common.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {

        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAsync(Func<TEntity, bool> predicate);
        Task<IQueryable<TEntity>> GetQueryAsync(Func<TEntity, bool> predicate);
        IQueryable<TEntity> GetAllQuery();
        Task<TEntity> GetByIdAsync(object id);
        Task InsertAsync(TEntity entity);
        Task InsertRangeAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(object id);
        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate);
        Task UpdateRangeAsync(IEnumerable<TEntity> entities);
        Task RemoveRangeAsync(IEnumerable<TEntity> entities);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    }
}