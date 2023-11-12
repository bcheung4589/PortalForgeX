using PortalForgeX.Domain.Entities.Tenants;

namespace PortalForgeX.Application.Data;

/// <summary>
/// Simple Factory to create DomainContext instances that can be used for
/// e.g. Database Migrations.
/// </summary>
public interface IDomainContextFactory
{
    /// <summary>
    /// Create and provide a DomainContext with the given <paramref name="connectionString"/>.
    /// </summary>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    IDomainContext CreateDbContext(string connectionString);

    /// <summary>
    /// Create and provide a DomainContext based on the given <paramref name="tenant"/>.
    /// </summary>
    /// <param name="tenant"></param>
    /// <returns></returns>
    IDomainContext CreateDbContext(Tenant tenant);
}
