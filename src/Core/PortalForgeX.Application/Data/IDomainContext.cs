using Microsoft.EntityFrameworkCore;
using PortalForgeX.Domain.Entities;

namespace PortalForgeX.Application.Data;

public interface IDomainContext
{
    // Entities
    DbSet<Client> Clients { get; set; }
    DbSet<ClientContact> ClientContacts { get; set; }
    DbSet<BusinessLocation> BusinessLocations { get; set; }
    DbSet<Checkout> Checkouts { get; set; }
    DbSet<Payment> Payments { get; set; }
    DbSet<UserProfile> UserProfiles { get; set; }
    DbSet<UserGroup> UserGroups { get; set; }
    DbSet<UserInGroup> UserInGroups { get; set; }
    DbSet<UserAccess> UserAccess { get; set; }
    DbSet<GroupAccess> GroupAccess { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task MigrateAsync(CancellationToken cancellationToken = default);
}
