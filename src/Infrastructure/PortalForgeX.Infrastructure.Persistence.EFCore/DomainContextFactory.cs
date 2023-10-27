using Microsoft.EntityFrameworkCore;
using PortalForgeX.Application.Data;

namespace PortalForgeX.Persistence.EFCore;

/// <inheritdoc/>
public class DomainContextFactory : IDomainContextFactory
{
    /// <inheritdoc/>
    public IDomainContext CreateDomainContext(string connectionString)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DomainContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new DomainContext(optionsBuilder.Options);
    }
}
