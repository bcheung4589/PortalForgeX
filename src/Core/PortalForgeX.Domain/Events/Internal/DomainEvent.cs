using MediatR;

namespace PortalForgeX.Domain.Events.Internal;

public abstract class DomainEvent : IDomainEvent, INotification
{
    public bool IsPublished { get; set; }
    public DateTimeOffset OccurredDate { get; protected set; }

    protected DomainEvent()
    {
        OccurredDate = DateTimeOffset.UtcNow;
    }
}
