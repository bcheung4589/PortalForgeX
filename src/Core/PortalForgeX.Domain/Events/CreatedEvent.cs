using PortalForgeX.Domain.Entities.Internal;
using PortalForgeX.Domain.Events.Internal;

namespace PortalForgeX.Domain.Events;

public class CreatedEvent<T> : DomainEvent where T : IEntity
{
    public T Entity { get; }

    public CreatedEvent(T entity)
    {
        Entity = entity;
    }
}