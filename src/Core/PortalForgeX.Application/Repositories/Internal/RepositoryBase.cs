using Microsoft.EntityFrameworkCore;
using PortalForgeX.Application.Data;
using PortalForgeX.Domain.Entities.Internal;
using System.Linq.Expressions;

namespace PortalForgeX.Application.Repositories.Internal;

/// <summary>
/// Base class for domain entity repositories with default implementations.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TPrimaryKey"></typeparam>
public abstract class RepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
    where TEntity : class, IEntity<TPrimaryKey>
{
    #region [ Abstractions ]

    /// <inheritdoc />
    public abstract IQueryable<TEntity> GetAsQuery();

    /// <inheritdoc />
    public abstract Task<EntityPage<TEntity>> GetPageAsync(EntityPageSetting settings, CancellationToken cancellationToken = default);

    /// <inheritdoc />
    public abstract Task<TEntity?> FindAsync(TPrimaryKey id, CancellationToken cancellationToken = default);

    /// <inheritdoc />
    public abstract Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <inheritdoc />
    public abstract Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <inheritdoc />
    public abstract Task<int> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <inheritdoc />
    public abstract Task<int> DeleteAsync(TPrimaryKey id, CancellationToken cancellationToken = default);

    #endregion [ Abstractions ]

    /// <inheritdoc />
    public virtual async Task<IList<TEntity>> GetAsListAsync(CancellationToken cancellationToken = default)
        => await GetAsQuery()
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

    /// <inheritdoc />
    public virtual async Task<IList<TEntity>> GetAsListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => await GetAsQuery()
            .Where(predicate)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

    /// <inheritdoc />
    public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => await GetAsQuery()
            .FirstOrDefaultAsync(predicate, cancellationToken);

    /// <inheritdoc />
    public virtual async Task<TEntity?> GetByIdAsync(TPrimaryKey id, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.Id != null && x.Id.Equals(id), cancellationToken);

    /// <inheritdoc />
    public virtual int Count()
        => GetAsQuery().Count();

    /// <inheritdoc />
    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
        => await GetAsQuery().CountAsync(cancellationToken);

    /// <inheritdoc />
    public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        => GetAsQuery().Where(predicate).Count();

    /// <inheritdoc />
    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => await GetAsQuery().Where(predicate).CountAsync(cancellationToken);

    /// <inheritdoc />
    public virtual long LongCount()
        => GetAsQuery().LongCount();

    /// <inheritdoc />
    public virtual async Task<long> LongCountAsync(CancellationToken cancellationToken = default)
        => await GetAsQuery().LongCountAsync(cancellationToken);

    /// <inheritdoc />
    public virtual long LongCount(Expression<Func<TEntity, bool>> predicate)
        => GetAsQuery().Where(predicate).LongCount();

    /// <inheritdoc />
    public virtual async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => await GetAsQuery().Where(predicate).LongCountAsync(cancellationToken);
}
