using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PortalForgeX.Application.Data;
using PortalForgeX.Domain.Entities.Identity;
using PortalForgeX.Domain.Entities.Tenants;
using PortalForgeX.Persistence.EFCore.Configurations;

namespace PortalForgeX.Persistence.EFCore;

public class PortalContext(DbContextOptions<PortalContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options), IPortalContext
{
    #region [ DbSet Models ]

    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantSettings> TenantSettings { get; set; }

    #endregion [ DbSet Models ]

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tenant>().ToTable("Tenants");
        modelBuilder.Entity<TenantSettings>().ToTable("TenantSettings");

        // Apply Portal Table Configurations
        new TenantConfiguration().Configure(modelBuilder.Entity<Tenant>());
    }
}
