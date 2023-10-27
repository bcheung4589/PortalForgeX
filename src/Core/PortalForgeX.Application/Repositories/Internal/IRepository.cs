using PortalForgeX.Application.Data;
using PortalForgeX.Domain.Entities.Internal;
using System.Linq.Expressions;

namespace PortalForgeX.Application.Repositories.Internal;

/// <summary>
/// Repository interface with the Primary Key as type int.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IRepository<TEntity> : IRepository<TEntity, int>
    where TEntity : class, IEntity<int>
{ }

/// <summary>
/// Repository interface for basic crud actions.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TPrimaryKey"></typeparam>
public interface IRepository<TEntity, TPrimaryKey>
    where TEntity : class, IEntity<TPrimaryKey>
{
    /// <summary>
    /// Get all the entities as Queryable.
    /// </summary>
    /// <returns></returns>
    IQueryable<TEntity> GetAsQuery();

    /// <summary>
    /// Get entities as EntityPage with pagination information. (Entities are not tracked by EF)
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<EntityPage<TEntity>> GetPageAsync(EntityPageSetting settings, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all the entities as List.
    /// </summary>
    /// <returns></returns>
    Task<IList<TEntity>> GetAsListAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all the entities as List.
    /// </summary>
    /// <param name="predicate">condition</param>
    /// <returns></returns>
    Task<IList<TEntity>> GetAsListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get entity by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<TEntity?> GetByIdAsync(TPrimaryKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the first or default entity.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the entity based on its primary key. This will NOT include related objects.
    /// </summary>
    /// <param name="keyValues"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TEntity?> FindAsync(TPrimaryKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Insert the given entity.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update with given entity.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete the given entity.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>The total number of rows deleted in the database.</returns>
    Task<int> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete the given entity based on its primary key.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The total number of rows deleted in the database.</returns>
    Task<int> DeleteAsync(TPrimaryKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Count the result set.
    /// </summary>
    /// <returns></returns>
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Count the result set with given condition.
    /// </summary>
    /// <param name="predicate">condition</param>
    /// <returns></returns>
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
}
