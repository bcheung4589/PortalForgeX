using Microsoft.EntityFrameworkCore;
using PortalForgeX.Domain.Entities.Tenants;

namespace PortalForgeX.Application.Data;

public interface IPortalContext
{
    DbSet<Tenant> Tenants { get; set; }
    DbSet<TenantSettings> TenantSettings { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
