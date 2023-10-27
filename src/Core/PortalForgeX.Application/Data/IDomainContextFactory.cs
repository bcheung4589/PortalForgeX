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
    IDomainContext CreateDomainContext(string connectionString);
}
