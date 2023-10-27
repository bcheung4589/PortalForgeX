using Microsoft.EntityFrameworkCore;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Repositories.Internal;
using PortalForgeX.Domain.Entities.Internal;
using PortalForgeX.Persistence.EFCore;

namespace PortalForgeX.Persistence.EFCore.Repositories.Internal;

public class EfRepositoryBase<TEntity> : EfRepositoryBase<TEntity, int>
    where TEntity : class, IEntity<int>
{
    public EfRepositoryBase(DomainContext context) : base(context) { }
}

public class EfRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<DomainContext, TEntity, TPrimaryKey>
    where TEntity : class, IEntity<TPrimaryKey>
{
    public EfRepositoryBase(DomainContext context) : base(context) { }
}

public class EfRepositoryBase<TDbContext, TEntity, TPrimaryKey> : RepositoryBase<TEntity, TPrimaryKey>
    where TDbContext : DomainContext
    where TEntity : class, IEntity<TPrimaryKey>
{
    /// <summary>
    /// The Database Context for this repository.
    /// </summary>
    public virtual TDbContext Context { get; }

    /// <summary>
    /// Table representation of the entities in the Context.
    /// </summary>
    protected virtual DbSet<TEntity> Table => Context.Set<TEntity>();

    public EfRepositoryBase(TDbContext context)
        => Context = context;

    /// <inheritdoc />
    public override IQueryable<TEntity> GetAsQuery()
        => Table;

    /// <inheritdoc />
    public override async Task<EntityPage<TEntity>> GetPageAsync(EntityPageSetting settings, CancellationToken cancellationToken = default)
        => await (await EntityPage<TEntity>.Create(Table.OrderBy(x => x.Id))
            .ApplyAsync(settings, cancellationToken))
            .ExecuteAsync(cancellationToken: cancellationToken);

    /// <inheritdoc />
    public override async Task<TEntity?> FindAsync(TPrimaryKey id, CancellationToken cancellationToken = default)
        => await Table.FindAsync(new object?[] { id, cancellationToken }, cancellationToken: cancellationToken);

    /// <inheritdoc />
    public override async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        => (await Table.AddAsync(entity, cancellationToken)).Entity;

    /// <inheritdoc />
    public override Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (!Table.Local.Contains(entity))
        {
            Table.Attach(entity);
        }

        Context.Entry(entity).State = EntityState.Modified;

        return Task.FromResult(entity);
    }

    /// <summary>
    /// Deleting the entity executes immediately against the database, rather than being deferred until <see cref="DbContext.SaveChanges()" /> is called.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The total number of rows deleted in the database.</returns>
    public override async Task<int> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await Table.Where(x => x.Id!.Equals(entity.Id)).ExecuteDeleteAsync(cancellationToken: cancellationToken);

    /// <summary>
    /// Deleting the entity executes immediately against the database, rather than being deferred until <see cref="DbContext.SaveChanges()" /> is called.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The total number of rows deleted in the database.</returns>
    public override async Task<int> DeleteAsync(TPrimaryKey id, CancellationToken cancellationToken = default)
        => await Table.Where(x => x.Id!.Equals(id)).ExecuteDeleteAsync(cancellationToken: cancellationToken);
}
