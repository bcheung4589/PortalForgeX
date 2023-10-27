using PortalForgeX.Domain.Entities.Internal;
using PortalForgeX.Domain.Events.Internal;

namespace PortalForgeX.Domain.Events;

public class UpdatedEvent<T> : DomainEvent where T : IEntity
{
    public T Entity { get; }

    public UpdatedEvent(T entity)
    {
        Entity = entity;
    }
}