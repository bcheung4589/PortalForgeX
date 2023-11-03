using PortalForgeX.Domain.Entities.Tenants;
using PortalForgeX.Domain.Services;

namespace PortalForgeX.Application.Tenants;

/// <inheritdoc/>
public class TenantConnectionProvider(string connectionStringFormat) : ITenantConnectionProvider
{
    private readonly string _connectionStringFormat = connectionStringFormat;

    /// <inheritdoc/>
    public string Provide(Tenant? tenant)
    {
        if (tenant is null)
        {
            return string.Format(_connectionStringFormat, "");
        }

        return string.Format(_connectionStringFormat, tenant.InternalName);
    }
}
