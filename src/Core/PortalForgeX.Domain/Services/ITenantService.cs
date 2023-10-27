using PortalForgeX.Domain.Entities;
using PortalForgeX.Domain.Entities.Tenants;
using PortalForgeX.Domain.Enums;

namespace PortalForgeX.Domain.Services;

/// <summary>
/// The TenantService provides all functionality regarding Tenants from CRUD-actions to the Review-actions.
/// </summary>
public interface ITenantService
{
    /// <summary>
    /// Get all the Tenants.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Tenant>> GetAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get Tenant by Id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create new tenant.
    /// </summary>
    /// <param name="tenant"></param>
    /// <returns></returns>
    Task<Tenant?> CreateAsync(Tenant tenant, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update the tenant.
    /// </summary>
    /// <param name="tenant"></param>
    /// <returns></returns>
    Task<Tenant?> UpdateAsync(Tenant tenant, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update the tenant to the new status.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="newStatus"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> UpdateStatusAsync(Guid id, TenantStatus newStatus, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete the Tenant.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Review: Approve the new Tenant.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> ApproveTenantAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Review: Reject the new Tenant.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> RejectTenantAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Push user to Tenant by creating a profile for the user at the tenant.
    /// DbMigrated Tenants will be set to ready when the first Tenant Admin is pushed.
    /// </summary>
    /// <param name="tenant"></param>
    /// <param name="newProfile"></param>
    /// <returns></returns>
    Task<bool> PushUserAsync(Tenant tenant, UserProfile newProfile, CancellationToken cancellationToken = default);
}
