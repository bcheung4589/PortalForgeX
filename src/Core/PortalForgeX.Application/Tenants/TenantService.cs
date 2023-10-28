using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Domain.Entities.Identity;
using PortalForgeX.Domain.Entities.Internal;
using PortalForgeX.Domain.Entities.Tenants;
using PortalForgeX.Domain.Enums;
using PortalForgeX.Domain.Events;
using PortalForgeX.Shared.Constants;
using PortalForgeX.Shared.Features.Tenants;

namespace PortalForgeX.Application.Tenants;

/// <inheritdoc/>
public class TenantService(
    ILogger<TenantService> logger,
    IPortalContext portalContext,
    IDomainContextFactory domainContextFactory,
    UserManager<ApplicationUser> userManager,
    ISender sender,
    IMapper mapper
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

        return changes > 0 ? await GetByIdAsync(entity.Id, cancellationToken) : null;
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

        return changes > 0 ? await GetByIdAsync(foundEntity.Id, cancellationToken) : null;
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

    #region [ User Profiles ]

    /// <inheritdoc/>
    public async Task<IEnumerable<TenantUserViewModel>?> GetTenantUsers(Tenant tenant, CancellationToken cancellationToken = default)
    {
        var foundEntity = await portalContext.Tenants.FindAsync([tenant.Id], cancellationToken: cancellationToken);
        if (foundEntity is null)
        {
            return null;
        }

        var tenantUsers = await userManager.Users.Where(x => x.TenantId == tenant.Id).ToListAsync(cancellationToken: cancellationToken);
        var tenantUsersViews = mapper.Map<IEnumerable<TenantUserViewModel>>(tenantUsers); // map ApplicationUser

        var domainContext = domainContextFactory.CreateDomainContext(tenant);
        var tenantUsersIds = tenantUsers.Select(x => x.Id).ToList();
        var userProfiles = await domainContext.UserProfiles
            .Include(x => x.Groups)
            .Where(x => tenantUsersIds.Contains(x.UserId))
            .ToListAsync(cancellationToken: cancellationToken);
        mapper.Map(userProfiles, tenantUsersViews); // map TenantUserProfile

        return tenantUsersViews;
    }

    /// <inheritdoc/>
    public async Task<bool> CreateTenantUserAsync(Tenant tenant, TenantUserProfile newProfile, CancellationToken cancellationToken = default)
    {
        var foundEntity = await portalContext.Tenants.FindAsync([tenant.Id], cancellationToken: cancellationToken);
        if (foundEntity is null)
        {
            return false;
        }

        var foundUser = await userManager.FindByIdAsync(newProfile.UserId.ToString());
        if (foundUser is null)
        {
            return false;
        }

        var domainContext = domainContextFactory.CreateDomainContext(tenant);
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

    /// <inheritdoc/>
    public async Task<bool> UpdateTenantUserAsync(Tenant tenant, TenantUserProfile updateProfile, CancellationToken cancellationToken = default)
    {
        var foundEntity = await portalContext.Tenants.FindAsync([tenant.Id], cancellationToken: cancellationToken);
        if (foundEntity is null)
        {
            return false;
        }

        var domainContext = domainContextFactory.CreateDomainContext(tenant);
        var tenantProfile = await domainContext.UserProfiles.FindAsync([updateProfile.UserId], cancellationToken: cancellationToken);
        if (tenantProfile is null)
        {
            return false;
        }

        tenantProfile.Title = updateProfile.Title;
        var result = await domainContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteTenantUserAsync(Tenant tenant, string userId, CancellationToken cancellationToken = default)
    {
        var foundEntity = await portalContext.Tenants.FindAsync([tenant.Id], cancellationToken: cancellationToken);
        if (foundEntity is null)
        {
            return false;
        }

        var domainContext = domainContextFactory.CreateDomainContext(tenant);

        var result = await domainContext.UserProfiles
            .Where(x => x.UserId.Equals(userId))
            .ExecuteDeleteAsync(cancellationToken);

        return result > 0;
    }

    /// <inheritdoc/>
    public async Task<int> AddUserToGroups(Tenant tenant, string userId, IEnumerable<int> groupIds, CancellationToken cancellationToken = default)
    {
        var foundEntity = await portalContext.Tenants.FindAsync([tenant.Id], cancellationToken: cancellationToken);
        if (foundEntity is null)
        {
            return 0;
        }

        var foundUser = await userManager.FindByIdAsync(userId);
        if (foundUser is null)
        {
            return 0;
        }

        var domainContext = domainContextFactory.CreateDomainContext(tenant);
        foreach (var groupId in groupIds)
        {
            if ((await domainContext.UserGroups.FindAsync([groupId], cancellationToken: cancellationToken)) == null)
            {
                continue;
            }

            await domainContext.UserInGroups.AddAsync(new Domain.Entities.UserInGroup
            {
                UserGroupId = groupId,
                UserId = userId
            }, cancellationToken);
        }

        return await domainContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<int> RemoveUserFromGroups(Tenant tenant, string userId, IEnumerable<int> groupIds, CancellationToken cancellationToken = default)
    {
        var foundEntity = await portalContext.Tenants.FindAsync([tenant.Id], cancellationToken: cancellationToken);
        if (foundEntity is null)
        {
            return 0;
        }

        var foundUser = await userManager.FindByIdAsync(userId);
        if (foundUser is null)
        {
            return 0;
        }

        var domainContext = domainContextFactory.CreateDomainContext(tenant);
        var result = await domainContext.UserInGroups
            .Where(x => x.UserId.Equals(userId) && groupIds.Contains(x.UserGroupId))
            .ExecuteDeleteAsync(cancellationToken: cancellationToken);

        return result;
    }

    #endregion [ User Profiles ]
}
