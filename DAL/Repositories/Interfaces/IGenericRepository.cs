using System.Linq.Expressions;

namespace DAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            int? skipCount = 0,
            int? takeCount = 0,
            params Expression<Func<TEntity, object>>[]? includeProperties);
        TEntity? GetByID(object id);
        TEntity Insert(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        TEntity Update(TEntity entityToUpdate);
        Task BulkInsertAsync(IEnumerable<TEntity> entities);
        IQueryable<TEntity> GetQueryable();
    }
}
