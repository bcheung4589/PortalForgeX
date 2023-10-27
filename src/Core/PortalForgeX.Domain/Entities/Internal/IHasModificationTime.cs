namespace PortalForgeX.Domain.Entities.Internal;

public interface IHasModificationTime
{
    /// <summary>
    /// Last time the object was modified.
    /// </summary>
    DateTime? LastModificationTime { get; set; }
}
