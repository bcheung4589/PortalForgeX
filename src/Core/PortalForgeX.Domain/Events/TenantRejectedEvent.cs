using PortalForgeX.Domain.Entities.Tenants;
using PortalForgeX.Domain.Events.Internal;

namespace PortalForgeX.Domain.Events;

public class TenantRejectedEvent(Tenant tenant) : DomainEvent
{
    public Tenant Tenant { get; set; } = tenant;
}
