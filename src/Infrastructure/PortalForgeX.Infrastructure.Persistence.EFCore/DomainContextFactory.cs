using Microsoft.EntityFrameworkCore;
using PortalForgeX.Application.Data;
using PortalForgeX.Domain.Entities.Tenants;
using PortalForgeX.Domain.Services;

namespace PortalForgeX.Persistence.EFCore;

/// <inheritdoc/>
public class DomainContextFactory(ITenantConnectionProvider tenantConnectionProvider) : IDomainContextFactory
{
    /// <inheritdoc/>
    public IDomainContext CreateDomainContext(string connectionString)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DomainContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new DomainContext(optionsBuilder.Options);
    }

    /// <inheritdoc/>
    public IDomainContext CreateDomainContext(Tenant tenant)
        => CreateDomainContext(tenantConnectionProvider.Provide(tenant));
}
