using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PortalForgeX.Persistence.EFCore.Internal;

/// <summary>
/// This class is needed to run EF Core PMC commands. Not used anywhere else
/// </summary>
public class DesignTimeDomainContextFactory : IDesignTimeDbContextFactory<DomainContext>
{
    public DomainContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DomainContext>();
        optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;database=PFX_Tenant;TrustServerCertificate=True;user=pfx_tenant;password=P@SSW0rd,!");
        return new(optionsBuilder.Options);
    }
}

public class DesignTimePortalContextFactory : IDesignTimeDbContextFactory<PortalContext>
{
    public PortalContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PortalContext>();
        optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;database=PFX_Portal;TrustServerCertificate=True;user=pfx_user;password=P@SSW0rd,!");

        return new(optionsBuilder.Options);
    }
}