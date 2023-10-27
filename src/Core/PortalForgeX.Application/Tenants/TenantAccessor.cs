using PortalForgeX.Domain.Entities.Tenants;

namespace PortalForgeX.Application.Tenants;

/// <summary>
/// Simple service for holding the current Tenant.
/// </summary>
public class TenantAccessor
{
    public Tenant? CurrentTenant { get; set; }

    public bool HasTenant => CurrentTenant is not null;
}
