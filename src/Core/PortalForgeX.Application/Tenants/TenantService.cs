using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Domain.Entities;
using PortalForgeX.Domain.Entities.Identity;
using PortalForgeX.Domain.Entities.Tenants;
using PortalForgeX.Domain.Enums;
using PortalForgeX.Domain.Events;
using PortalForgeX.Domain.Services;
using PortalForgeX.Shared.Constants;

namespace PortalForgeX.Application.Tenants;

/// <inheritdoc/>
public class TenantService(
    ILogger<TenantService> logger,
    IPortalContext portalContext,
    IDomainContext domainContext,
    UserManager<ApplicationUser> userManager,
    ISender sender
    ) : ITenantService
{
    private readonly ILogger<TenantService> _logger = logger;

    #region [ CRUD ]

    /// <inheritdoc/>
    public async Task<IEnumerable<Tenant>> GetAsync(CancellationToken cancellationToken = default)
        => await portalContext.Tenants.Include(x => x.Manager).ToListAsync(cancellationToken: cancellationToken);

    /// <inheritdoc/>
    public async Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await portalContext.Tenants.Include(x => x.Manager).FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

    /// <inheritdoc/>
    public async Task<Tenant?> CreateAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        tenant.Id = Guid.NewGuid();
        tenant.CreationTime = DateTime.UtcNow;
        tenant.Status = TenantStatus.Created;

        var entity = (await portalContext.Tenants.AddAsync(tenant, cancellationToken)).Entity;
        var changes = await portalContext.SaveChangesAsync(cancellationToken);

        return changes > 0 ? entity : null;
    }

    /// <inheritdoc/>
    public async Task<Tenant?> UpdateAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        var foundEntity = await portalContext.Tenants.FindAsync([tenant.Id], cancellationToken: cancellationToken);
        if (foundEntity is null)
        {
            return null;
        }

        foundEntity.ExternalId = tenant.ExternalId;
        foundEntity.Name = tenant.Name;
        foundEntity.InternalName = tenant.InternalName;
        foundEntity.Status = tenant.Status;
        foundEntity.Host = tenant.Host;
        foundEntity.IsActive = tenant.IsActive;
        foundEntity.Remarks = tenant.Remarks;
        foundEntity.TenantSettingsId = tenant.TenantSettingsId;
        foundEntity.LastModificationTime = DateTime.UtcNow;

        var changes = await portalContext.SaveChangesAsync(cancellationToken);

        return changes > 0 ? foundEntity : null;
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateStatusAsync(Guid id, TenantStatus newStatus, CancellationToken cancellationToken = default)
    {
        var foundEntity = await portalContext.Tenants.FindAsync([id], cancellationToken: cancellationToken);
        if (foundEntity is null)
        {
            return false;
        }

        foundEntity.Status = newStatus;

        var changes = await portalContext.SaveChangesAsync(cancellationToken);

        return changes > 0;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var foundEntity = await portalContext.Tenants.FindAsync([id], cancellationToken: cancellationToken);
        if (foundEntity is null)
        {
            return false;
        }

        var changes = await portalContext.Tenants
            .Where(x => x.Id.Equals(id))
            .ExecuteDeleteAsync(cancellationToken);

        return changes > 0;
    }

    #endregion [ CRUD ]

    #region [ Review ]

    /// <inheritdoc/>
    public async Task<bool> ApproveTenantAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var foundEntity = await GetByIdAsync(id, cancellationToken);
        if (foundEntity is null)
        {
            return false;
        }

        foundEntity.Status = TenantStatus.Approved;

        var succeeded = await portalContext.SaveChangesAsync(cancellationToken) > 0;
        if (succeeded)
        {
            /// <see cref="TenantReviewEventHandler"/>
            await sender.Send(new TenantApprovedEvent(foundEntity), cancellationToken);
        }

        return succeeded;
    }

    /// <inheritdoc/>
    public async Task<bool> RejectTenantAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var foundEntity = await GetByIdAsync(id, cancellationToken);
        if (foundEntity is null)
        {
            return false;
        }

        foundEntity.Status = TenantStatus.Rejected;

        var succeeded = await portalContext.SaveChangesAsync(cancellationToken) > 0;
        if (succeeded)
        {
            /// <see cref="TenantReviewEventHandler"/>
            await sender.Send(new TenantRejectedEvent(foundEntity), cancellationToken);
        }

        return succeeded;
    }

    #endregion [ Review ]

    /// <inheritdoc/>
    public async Task<bool> PushUserAsync(Tenant tenant, UserProfile newProfile, CancellationToken cancellationToken = default)
    {
        // ensure tenant exist
        var foundEntity = await portalContext.Tenants.FindAsync([tenant.Id], cancellationToken: cancellationToken);
        if (foundEntity is null)
        {
            return false;
        }

        // ensure user exist
        var foundUser = await userManager.FindByIdAsync(newProfile.UserId.ToString());
        if (foundUser is null)
        {
            return false;
        }

        // add user profile to tenant db
        newProfile.CreationTime = DateTime.UtcNow;
        await domainContext.UserProfiles.AddAsync(newProfile, cancellationToken);
        var result = await domainContext.SaveChangesAsync(cancellationToken);
        if (result < 1)
        {
            return false;
        }

        // move tenant status to ready if it contains atleast 1 tenant admin user
        if (tenant.Status == TenantStatus.DbMigrated && await userManager.IsInRoleAsync(foundUser, SystemRolesNames.TENANT_ADMIN))
        {
            foundEntity.Status = TenantStatus.Ready;

            _ = await portalContext.SaveChangesAsync(cancellationToken);
        }

        return true;
    }
}
