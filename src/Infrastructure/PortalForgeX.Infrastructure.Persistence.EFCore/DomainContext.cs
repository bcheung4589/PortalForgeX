using Microsoft.EntityFrameworkCore;
using PortalForgeX.Application.Data;
using PortalForgeX.Domain.Entities;
using PortalForgeX.Persistence.EFCore.Configurations;
using PortalForgeX.Persistence.EFCore.Internal;
using System.Reflection;

namespace PortalForgeX.Persistence.EFCore;

/// <summary>
/// The Database Context for all the Domain objects.
/// </summary>
public class DomainContext(DbContextOptions<DomainContext> options) : BaseModelContext(options), IDomainContext
{
    #region [ DbSet Models ]

    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<ClientContact> ClientContacts { get; set; } = null!;
    public DbSet<BusinessLocation> BusinessLocations { get; set; } = null!;
    public DbSet<Checkout> Checkouts { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
    public DbSet<UserProfile> UserProfiles { get; set; } = null!;
    public DbSet<UserGroup> UserGroups { get; set; } = null!;
    public DbSet<UserInGroup> UserInGroups { get; set; } = null!;
    public DbSet<UserAccess> UserAccess { get; set; } = null!;
    public DbSet<GroupAccess> GroupAccess { get; set; } = null!;

    #endregion [ DbSet Models ]

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Retrieve Entities
        var domainModel = typeof(Client);
        var modelTypes = Assembly.GetAssembly(domainModel)!
                                .GetTypes()
                                .Where(t => t.FullName!.StartsWith(domainModel.Namespace!)
                                            && t.IsClass && !t.IsSealed && !t.IsAbstract
                                            && t.Namespace == domainModel.Namespace);

        // Handle plural names
        foreach (var modelType in modelTypes)
        {
            if (!modelType.Name.EndsWith("s"))
            {
                // Entities to Table
                modelBuilder.Entity(modelType)
                            .ToTable(modelType.Name + "s");
            }
            else
            {
                modelBuilder.Entity(modelType)
                            .ToTable(modelType.Name);
            }
        }

        // Apply Domain Table Configurations
        new ClientConfiguration().Configure(modelBuilder.Entity<Client>());
        new PaymentConfiguration().Configure(modelBuilder.Entity<Payment>());
        new CheckoutConfiguration().Configure(modelBuilder.Entity<Checkout>());
        new BusinessLocationConfiguration().Configure(modelBuilder.Entity<BusinessLocation>());
        new UserProfileConfiguration().Configure(modelBuilder.Entity<UserProfile>());
        new UserGroupConfiguration().Configure(modelBuilder.Entity<UserGroup>());
        new UserAccessConfiguration().Configure(modelBuilder.Entity<UserAccess>());
        new GroupAccessConfiguration().Configure(modelBuilder.Entity<GroupAccess>());
    }

    public async Task MigrateAsync(CancellationToken cancellationToken = default)
        => await Database.MigrateAsync(cancellationToken: cancellationToken);
}
