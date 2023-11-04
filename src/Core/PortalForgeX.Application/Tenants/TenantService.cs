using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Domain.Entities.Identity;
using PortalForgeX.Domain.Entities.Tenants;
using PortalForgeX.Domain.Enums;
using PortalForgeX.Domain.Events;
using PortalForgeX.Shared.Constants;
using PortalForgeX.Shared.Extensions;
using PortalForgeX.Shared.Features.Tenants;

namespace PortalForgeX.Application.Tenants;

/// <inheritdoc/>
public class TenantService(
    ILogger<TenantService> logger,
    IPortalContext portalContext,
    IDomainContextFactory domainContextFactory,
    UserManager<ApplicationUser> userManager,
    IMediator sender,
    IMapper mapper
    ) : ITenantService
{
    private readonly ILogger<TenantService> _logger = logger;

    public List<string> Errors { get; } = [];

    #region [ Tenants ]

    /// <inheritdoc/>
    public async Task<IEnumerable<Tenant>> GetAsync(CancellationToken cancellationToken = default)
        => await portalContext.Tenants
            .Include(x => x.Manager)
            .ToListAsync(cancellationToken: cancellationToken);

    /// <inheritdoc/>
    public async Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await portalContext.Tenants
            .Include(x => x.Manager)
            .Include(x => x.TenantSettings)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

    /// <inheritdoc/>
    public async Task<Tenant?> CreateAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        tenant.Id = Guid.NewGuid();
        tenant.CreationTime = DateTime.UtcNow;
        tenant.Status = TenantStatus.Created;
        tenant.TenantSettings.Id = Guid.NewGuid();
        tenant.TenantSettings.CreationTime = DateTime.UtcNow;

        if (string.IsNullOrWhiteSpace(tenant.InternalName))
        {
            var internalName = tenant.Name.SanitizeAlphaNum();
            if (internalName.Contains(' '))
            {
                do
                {
                    internalName = internalName.Replace(" ", "_");
                }
                while (internalName.Contains(' '));
            }

            tenant.InternalName = internalName.ToLower();
        }

        var entity = (await portalContext.Tenants.AddAsync(tenant, cancellationToken)).Entity;
        var changes = await portalContext.SaveChangesAsync(cancellationToken);

        return changes > 0 ? await GetByIdAsync(entity.Id, cancellationToken) : null;
    }

    /// <inheritdoc/>
    public async Task<Tenant?> UpdateAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        var dbTenant = await portalContext.Tenants.FindAsync([tenant.Id], cancellationToken: cancellationToken);
        if (dbTenant is null)
        {
            Errors.Add($"Tenant not found. ({tenant.Name}).");
            _logger.LogError("Tenant not found [{Name}/{InternalName}/{Id}].", tenant.Name, tenant.InternalName, tenant.Id);
            return null;
        }

        dbTenant.ExternalId = tenant.ExternalId;
        dbTenant.Name = tenant.Name;
        dbTenant.InternalName = tenant.InternalName;
        dbTenant.Status = tenant.Status;
        dbTenant.Host = tenant.Host;
        dbTenant.IsActive = tenant.IsActive;
        dbTenant.Remarks = tenant.Remarks;
        dbTenant.LastModificationTime = DateTime.UtcNow;

        dbTenant.TenantSettings.LogoUrl = tenant.TenantSettings.LogoUrl;
        dbTenant.TenantSettings.Brand = tenant.TenantSettings.Brand;
        dbTenant.TenantSettings.PrimaryColor = tenant.TenantSettings.PrimaryColor;
        dbTenant.TenantSettings.SecondaryColor = tenant.TenantSettings.SecondaryColor;
        dbTenant.TenantSettings.IsPublicRegisterEnabled = tenant.TenantSettings.IsPublicRegisterEnabled;
        dbTenant.TenantSettings.AdditionalData = tenant.TenantSettings.AdditionalData;

        var changes = await portalContext.SaveChangesAsync(cancellationToken);

        return changes > 0 ? await GetByIdAsync(dbTenant.Id, cancellationToken) : null;
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateStatusAsync(Guid id, TenantStatus newStatus, CancellationToken cancellationToken = default)
    {
        var dbTenant = await portalContext.Tenants.FindAsync([id], cancellationToken: cancellationToken);
        if (dbTenant is null)
        {
            Errors.Add($"Tenant not found. ({id}).");
            _logger.LogError("Tenant not found [{id}].", id);
            return false;
        }

        dbTenant.Status = newStatus;
        dbTenant.LastModificationTime = DateTime.UtcNow;

        var changes = await portalContext.SaveChangesAsync(cancellationToken);

        return changes > 0;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbTenant = await portalContext.Tenants.FindAsync([id], cancellationToken: cancellationToken);
        if (dbTenant is null)
        {
            Errors.Add($"Tenant not found. ({id}).");
            _logger.LogError("Tenant not found [{id}].", id);
            return false;
        }

        var changes = await portalContext.Tenants
            .Where(x => x.Id.Equals(id))
            .ExecuteDeleteAsync(cancellationToken);

        return changes > 0;
    }

    #endregion [ Tenants ]

    #region [ Review ]

    /// <inheritdoc/>
    public async Task<bool> ApproveTenantAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tenant = await GetByIdAsync(id, cancellationToken);
        if (tenant is null)
        {
            Errors.Add($"Tenant not found. ({id}).");
            _logger.LogError("Tenant not found [{Id}].", id);
            return false;
        }

        tenant.Status = TenantStatus.Approved;

        var succeeded = await portalContext.SaveChangesAsync(cancellationToken) > 0;
        if (succeeded)
        {
            /// <see cref="TenantReviewEventHandler"/>
            await sender.Publish(new TenantApprovedEvent(tenant), cancellationToken);
        }

        return succeeded;
    }

    /// <inheritdoc/>
    public async Task<bool> RejectTenantAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tenant = await GetByIdAsync(id, cancellationToken);
        if (tenant is null)
        {
            Errors.Add($"Tenant not found. ({id}).");
            _logger.LogError("Tenant not found [{Id}].", id);
            return false;
        }

        tenant.Status = TenantStatus.Rejected;

        var succeeded = await portalContext.SaveChangesAsync(cancellationToken) > 0;
        if (succeeded)
        {
            /// <see cref="TenantReviewEventHandler"/>
            await sender.Publish(new TenantRejectedEvent(tenant), cancellationToken);
        }

        return succeeded;
    }

    #endregion [ Review ]

    #region [ User Profiles ]

    /// <inheritdoc/>
    public async Task<IEnumerable<TenantUserViewModel>?> GetTenantProfiles(Tenant tenant, CancellationToken cancellationToken = default)
    {
        var tenantUsers = await userManager.Users.Where(x => x.TenantId == tenant.Id).ToListAsync(cancellationToken: cancellationToken);
        if (tenantUsers.Count == 0)
        {
            return null;
        }

        var tenantUsersViews = mapper.Map<IEnumerable<TenantUserViewModel>>(tenantUsers);
        var domainContext = domainContextFactory.CreateDomainContext(tenant);
        var tenantUsersIds = tenantUsers.Select(x => x.Id).ToList();
        var userProfiles = await domainContext.UserProfiles
            .Include(x => x.Groups)
            .Where(x => tenantUsersIds.Contains(x.UserId))
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var userProfile in userProfiles)
        {
            var tenantUser = tenantUsersViews.FirstOrDefault(x => x.Id == userProfile.UserId);
            if (tenantUser is null)
            {
                continue;
            }

            mapper.Map(userProfile, tenantUser);
        }

        return tenantUsersViews;
    }

    public async Task<TenantUserFormModel?> ProvideTenantProfileForEdit(Tenant tenant, string userId, CancellationToken cancellationToken = default)
    {
        var tenantUser = await userManager.FindByIdAsync(userId);
        if (tenantUser is null)
        {
            return null;
        }

        var tenantUserView = mapper.Map<TenantUserFormModel>(tenantUser);
        var domainContext = domainContextFactory.CreateDomainContext(tenant);
        var userProfile = await domainContext.UserProfiles
            .Include(x => x.Groups)
            .FirstOrDefaultAsync(x => x.UserId == tenantUser.Id, cancellationToken: cancellationToken);

        mapper.Map(userProfile, tenantUserView);

        return tenantUserView;
    }

    /// <inheritdoc/>
    public async Task<bool> CreateTenantProfileAsync(Tenant tenant, TenantUserFormModel formModel, string password, IEnumerable<string>? roles = null, CancellationToken cancellationToken = default)
    {
        var dbTenant = await portalContext.Tenants.FindAsync([tenant.Id], cancellationToken: cancellationToken);
        if (dbTenant is null)
        {
            Errors.Add($"Tenant not found. ({tenant.Name}).");
            _logger.LogError("Tenant not found [{Name}/{InternalName}/{Id}].", tenant.Name, tenant.InternalName, tenant.Id);
            return false;
        }

        /// create portal user for login
        var appUser = mapper.Map<ApplicationUser>(formModel);

        appUser.CreationTime = DateTime.UtcNow;
        appUser.UserName = appUser.Email;
        appUser.EmailConfirmed = true;

        var resultUser = await userManager.CreateAsync(appUser, password);
        if (!resultUser.Succeeded)
        {
            Errors.Add("Failed creating user login.");
            _logger.LogError("Failed creating user login ([{Name}/{InternalName}/{Id}].", tenant.Name, tenant.InternalName, tenant.Id);
            return false;
        }

        var user = await userManager.FindByNameAsync(appUser.UserName!);
        if (user is null)
        {
            Errors.Add($"User not found. ({appUser.UserName})");
            return false;
        }

        if (roles is not null && roles.Any())
        {
            _ = await userManager.AddToRolesAsync(user, roles);
        }

        /// add profile to tenant for user
        formModel.Id = user.Id;
        var tenantProfile = mapper.Map<TenantUserProfile>(formModel);

        var domainContext = domainContextFactory.CreateDomainContext(tenant);
        await domainContext.UserProfiles.AddAsync(tenantProfile, cancellationToken);
        var result = await domainContext.SaveChangesAsync(cancellationToken);
        if (result < 1)
        {
            Errors.Add("Failed creating user profile.");
            _logger.LogError("Failed creating user profile [{Name}/{InternalName}/{Id}].", tenant.Name, tenant.InternalName, tenant.Id);
            return false;
        }

        /// move tenant status to ready if it contains atleast 1 tenant admin user
        if (tenant.Status == TenantStatus.DbMigrated && await userManager.IsInRoleAsync(user, SystemRolesNames.TENANT_ADMIN))
        {
            dbTenant.Status = TenantStatus.Ready;

            _ = await portalContext.SaveChangesAsync(cancellationToken);
        }

        return true;
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateTenantProfileAsync(Tenant tenant, TenantUserFormModel updateProfile, CancellationToken cancellationToken = default)
    {
        var updateUser = await userManager.FindByIdAsync(updateProfile.Id);
        if (updateUser is null)
        {
            Errors.Add($"User not found. ({updateProfile.Email}).");
            _logger.LogError("User not found ({Email}) [{Name}/{InternalName}/{Id}].", updateProfile.Email, tenant.Name, tenant.InternalName, tenant.Id);
            return false;
        }

        /// update user data
        mapper.Map(updateProfile, updateUser);
        updateUser.LastModificationTime = DateTime.UtcNow;
        var userUpdateResult = await userManager.UpdateAsync(updateUser);
        if (!userUpdateResult.Succeeded)
        {
            Errors.Add($"Failed updating user. ({updateProfile.Email}).");
            _logger.LogError("Failed updating user ({Email}) [{Name}/{InternalName}/{Id}].", updateProfile.Email, tenant.Name, tenant.InternalName, tenant.Id);
            return false;
        }

        var domainContext = domainContextFactory.CreateDomainContext(tenant);
        var tenantProfile = await domainContext.UserProfiles.FindAsync([updateProfile.Id], cancellationToken: cancellationToken);
        if (tenantProfile is null)
        {
            Errors.Add($"Profile not found. ({updateProfile.Email}).");
            _logger.LogError("Profile not found ({Email}) [{Name}/{InternalName}/{Id}].", updateProfile.Email, tenant.Name, tenant.InternalName, tenant.Id);

            return false;
        }

        /// update profile data
        tenantProfile.Title = updateProfile.Title;
        _ = await domainContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteTenantProfileAsync(Tenant tenant, string userId, CancellationToken cancellationToken = default)
    {
        var deleteUser = await userManager.FindByIdAsync(userId);
        if (deleteUser is null)
        {
            Errors.Add($"User not found. ({userId}).");
            _logger.LogError("User not found ({userId}) [{Name}/{InternalName}/{Id}].", userId, tenant.Name, tenant.InternalName, tenant.Id);
            return false;
        }

        var deleteResult = await userManager.DeleteAsync(deleteUser);
        if (!deleteResult.Succeeded)
        {
            Errors.Add($"Failed deleting user {deleteUser.Email}.");
            _logger.LogError("Failed deleting user ({Email}) [{Name}/{InternalName}/{Id}].", deleteUser.Email, tenant.Name, tenant.InternalName, tenant.Id);
            return false;
        }

        var domainContext = domainContextFactory.CreateDomainContext(tenant);

        var result = await domainContext.UserProfiles
            .Where(x => x.UserId.Equals(userId))
            .ExecuteDeleteAsync(cancellationToken);

        return result > 0;
    }

    #endregion [ User Profiles ]

}
