using PortalForgeX.Domain.Entities.Internal;
using PortalForgeX.Domain.Entities.Tenants;
using System.ComponentModel.DataAnnotations;

namespace PortalForgeX.Domain.Entities;

public class UserAccess : IEntity
{
    /// <summary>
    /// The user profile reference.
    /// </summary>
    public Guid UserProfileId { get; set; }

    /// <summary>
    /// UserProfile instance.
    /// </summary>
    public TenantUserProfile UserProfile { get; set; } = null!;

    /// <summary>
    /// The name of the feature/permission for this access control.
    /// </summary>
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// The UserId that granted or revoked the access.
    /// </summary>
    public Guid AuthorizedUserId { get; set; }

    /// <summary>
    /// Last time the UserAccess was modified.
    /// </summary>
    public DateTime? LastModificationTime { get; set; }

    /// <summary>
    /// UserAccess has a composite primary key.
    /// </summary>
    /// <returns>false</returns>
    public bool IsTransient() => false;
}
