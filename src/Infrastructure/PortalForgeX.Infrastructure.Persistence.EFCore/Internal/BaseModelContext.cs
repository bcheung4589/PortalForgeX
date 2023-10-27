using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PortalForgeX.Application.Exceptions;
using PortalForgeX.Domain.Entities.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace PortalForgeX.Persistence.EFCore.Internal;

/// <summary>
/// The BaseModelContext is an base class that handles 
/// Id's as Guid, CreationTime and LastModificationTime
/// before saving changes to the database.
/// </summary>
public abstract class BaseModelContext : DbContext
{
    public BaseModelContext(DbContextOptions options) : base(options) { }

    public override int SaveChanges()
    {
        try
        {
            StartProcessChangedEntries();
            return base.SaveChanges();
        }
        catch (DbUpdateException ex)
        {
            throw new UnitOfWorkException("Unable to Commit.", ex);
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        return await SaveChangesAsync(CancellationToken.None);
    }

    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            StartProcessChangedEntries();
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new UnitOfWorkException("Unable to Commit.", ex);
        }
    }

    protected virtual void StartProcessChangedEntries()
    {
        foreach (var entry in ChangeTracker.Entries().ToArray())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    CheckAndSetId(entry);
                    SetCreationAuditProperties(entry.Entity);
                    break;
                case EntityState.Modified:
                    SetModificationAuditProperties(entry);
                    break;
            }
        }
    }

    protected virtual void CheckAndSetId(EntityEntry entry)
    {
        //Set GUID Ids
        if (entry.Entity is IEntity<Guid> entity && entity.Id == Guid.Empty)
        {
            var dbGeneratedAttr = entity.Id.GetType().GetCustomAttribute<DatabaseGeneratedAttribute>();
            if (dbGeneratedAttr is null || dbGeneratedAttr.DatabaseGeneratedOption == DatabaseGeneratedOption.None)
            {
                entity.Id = Guid.NewGuid();
            }
        }
    }

    protected virtual void SetCreationAuditProperties(object entityAsObj)
    {
        if (entityAsObj is not IHasCreationTime entityWithCreationTime)
        {
            return;
        }

        if (entityWithCreationTime.CreationTime == default)
        {
            entityWithCreationTime.CreationTime = DateTime.UtcNow;
        }
    }

    protected virtual void SetModificationAuditProperties(EntityEntry entry)
    {
        if (typeof(IHasModificationTime).IsAssignableFrom(entry.Entity.GetType()))
        {
            ((IHasModificationTime)entry.Entity).LastModificationTime = DateTime.UtcNow;
        }
    }
}
