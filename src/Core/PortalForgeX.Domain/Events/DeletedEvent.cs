using PortalForgeX.Domain.Entities.Internal;
using PortalForgeX.Domain.Events.Internal;

namespace PortalForgeX.Domain.Events;

public class DeletedEvent<T> : DomainEvent where T : IEntity
{
    public T Entity { get; }

    public DeletedEvent(T entity)
    {
        Entity = entity;
    }
}
