using Microsoft.EntityFrameworkCore;
using PortalForgeX.Application.Data;
using PortalForgeX.Domain.Entities.Tenants;
using PortalForgeX.Domain.Services;

namespace PortalForgeX.Persistence.EFCore;

/// <inheritdoc/>
public class DomainContextFactory(ITenantConnectionProvider tenantConnectionProvider) : IDomainContextFactory
{
    /// <inheritdoc/>
    public IDomainContext CreateDbContext(string connectionString)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DomainContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new DomainContext(optionsBuilder.Options);
    }

    /// <inheritdoc/>
    public IDomainContext CreateDbContext(Tenant tenant)
        => CreateDbContext(tenantConnectionProvider.Provide(tenant));
}
