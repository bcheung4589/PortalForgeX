using PortalForgeX.Domain.Entities.Internal;

namespace PortalForgeX.Domain.Entities;

/// <summary>
/// Pivot class between UserProfile and UserGroup.
/// </summary>
public class UserInGroup : IEntity
{
    /// <summary>
    /// Tenant reference.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// UserGroup reference.
    /// </summary>
    public int UserGroupId { get; set; }

    /// <summary>
    /// UserInGroup has a composite primary key.
    /// </summary>
    /// <returns>false</returns>
    public bool IsTransient() => false;
}
