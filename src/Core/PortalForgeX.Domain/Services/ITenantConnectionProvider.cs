using PortalForgeX.Domain.Entities.Tenants;

namespace PortalForgeX.Domain.Services;

/// <summary>
/// Simple provider to retrieve the database connection for tenants.
/// </summary>
public interface ITenantConnectionProvider
{
    /// <summary>
    /// initialize the provider with the connection string format that uses the tenants.
    /// </summary>
    /// <param name="connectionStringFormat"></param>
    void RegisterFromConfig(string connectionStringFormat);

    /// <summary>
    /// Provide a connection string for this given <paramref name="tenant"/>.
    /// </summary>
    /// <param name="tenant"></param>
    /// <returns></returns>
    string Provide(Tenant? tenant);
}