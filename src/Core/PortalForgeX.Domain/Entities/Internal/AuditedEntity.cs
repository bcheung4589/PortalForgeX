namespace PortalForgeX.Domain.Entities.Internal;

public abstract class AuditedEntity<TPrimaryKey> : CreationEntity<TPrimaryKey>, IHasModificationTime
{
    /// <inheritdoc/>
    public virtual DateTime? LastModificationTime { get; set; }
}
