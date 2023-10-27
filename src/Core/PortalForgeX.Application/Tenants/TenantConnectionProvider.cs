using PortalForgeX.Domain.Entities.Tenants;
using PortalForgeX.Domain.Services;

namespace PortalForgeX.Application.Tenants;

/// <inheritdoc/>
public class TenantConnectionProvider : ITenantConnectionProvider
{
    private string _connectionStringFormat = string.Empty;

    /// <inheritdoc/>
    public void RegisterFromConfig(string connectionStringFormat)
    {
        _connectionStringFormat += connectionStringFormat;
    }

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
