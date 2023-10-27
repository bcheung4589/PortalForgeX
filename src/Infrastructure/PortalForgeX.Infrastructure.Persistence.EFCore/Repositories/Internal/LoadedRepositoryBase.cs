using PortalForgeX.Application.Repositories.Internal;
using PortalForgeX.Domain.Entities.Internal;
using PortalForgeX.Persistence.EFCore;

namespace PortalForgeX.Persistence.EFCore.Repositories.Internal;

public abstract class LoadedRepositoryBase<TEntity> : LoadedRepositoryBase<TEntity, int>
    where TEntity : class, IEntity<int>
{
    public LoadedRepositoryBase(DomainContext context) : base(context) { }
}

public abstract class LoadedRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<TEntity, TPrimaryKey>, IRepository<TEntity, TPrimaryKey>
    where TEntity : class, IEntity<TPrimaryKey>
{
    /// <summary>
    /// Switch to enable/disable the usage of the Included Joins.
    /// </summary>
    public bool UseIncludedTable { get; set; }

    public LoadedRepositoryBase(DomainContext context) : base(context)
        => UseIncludedTable = true;

    protected abstract IQueryable<TEntity> IncludedTable();

    /// <summary>
    /// Get the Query with the default joined tables.
    /// </summary>
    /// <returns></returns>
    public override IQueryable<TEntity> GetAsQuery()
        => UseIncludedTable ? IncludedTable() : Table;
}
