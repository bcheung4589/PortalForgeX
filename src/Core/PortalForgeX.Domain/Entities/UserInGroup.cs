using PortalForgeX.Domain.Entities.Internal;
using PortalForgeX.Domain.Entities.Tenants;

namespace PortalForgeX.Domain.Entities;

/// <summary>
/// Pivot class between UserProfile and UserGroup.
/// </summary>
public class UserInGroup : IEntity
{
    /// <summary>
    /// Profile reference.
    /// </summary>
    public string UserId { get; set; } = null!;

    /// <summary>
    /// Profile instance.
    /// </summary>
    public TenantUserProfile User { get; set; } = null!;

    /// <summary>
    /// UserGroup reference.
    /// </summary>
    public int UserGroupId { get; set; }

    /// <summary>
    /// UserGroup instance.
    /// </summary>
    public UserGroup UserGroup { get; set; } = null!;

    /// <summary>
    /// UserInGroup has a composite primary key.
    /// </summary>
    /// <returns>false</returns>
    public bool IsTransient() => false;
}
