namespace PortalForgeX.Domain.Entities.Internal;

public interface IEntity
{
    /// <summary>
    /// Checks if the Id-property can be compared based on Id being an valid Number.
    /// </summary>
    /// <returns></returns>
    bool IsTransient();
}

public interface IEntity<TPrimaryKey> : IEntity
{
    /// <summary>
    /// Primary key of the object.
    /// </summary>
    TPrimaryKey Id { get; set; }
}

