using PortalForgeX.Domain.Entities.Tenants;
using System.Security.Claims;

namespace PortalForgeX.Application.Tenants;

/// <summary>
/// Simple service for holding the current Tenant.
/// </summary>
public sealed class TenantAccessor(ITenantService tenantService)
{
    private readonly ITenantService _tenantService = tenantService;

    public Tenant? CurrentTenant { get; set; }

    public bool HasTenant => CurrentTenant is not null;

    /// <summary>
    /// Load the Tenant from the user claim.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task LoadFromAsync(ClaimsPrincipal user)
    {
        var tenantClaim = user.Claims.FirstOrDefault(x => x.Type == TenantClaimTypes.TenantId);
        if (tenantClaim == null || !Guid.TryParse(tenantClaim.Value, out var tenantId))
        {
            // invalid claim
            return;
        }

        if (CurrentTenant is not null && CurrentTenant.Id == tenantId)
        {
            // already loaded
            return;
        }

        CurrentTenant = await _tenantService.GetByIdAsync(tenantId);
    }
}
