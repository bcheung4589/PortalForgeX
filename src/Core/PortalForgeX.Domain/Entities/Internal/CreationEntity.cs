namespace PortalForgeX.Domain.Entities.Internal;

public abstract class CreationEntity<TPrimaryKey> : Entity<TPrimaryKey>, IHasCreationTime
{
    /// <inheritdoc/>
    public virtual DateTime CreationTime { get; set; }
}
