using PortalForgeX.Domain.Entities.Tenants;

namespace PortalForgeX.Domain.Services;

/// <summary>
/// Simple provider to retrieve the database connection for tenants.
/// </summary>
public interface ITenantConnectionProvider
{
    /// <summary>
    /// Provide a connection string for this given <paramref name="tenant"/>.
    /// </summary>
    /// <param name="tenant"></param>
    /// <returns></returns>
    string Provide(Tenant tenant);
}