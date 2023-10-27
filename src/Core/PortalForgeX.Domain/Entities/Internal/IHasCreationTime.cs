namespace PortalForgeX.Domain.Entities.Internal;

public interface IHasCreationTime
{
    /// <summary>
    /// The time the object was created.
    /// </summary>
    DateTime CreationTime { get; set; }
}