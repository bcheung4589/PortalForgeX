namespace PortalForgeX.Domain.Events.Internal;

public interface IDomainEvent
{
    bool IsPublished { get; set; }
    DateTimeOffset OccurredDate { get; }
}
