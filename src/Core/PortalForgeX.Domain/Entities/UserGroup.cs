using PortalForgeX.Domain.Entities.Internal;
using System.ComponentModel.DataAnnotations;

namespace PortalForgeX.Domain.Entities;

/// <summary>
/// User group on Tenant level for grouping of UserProfiles
/// </summary>
public class UserGroup : AuditedEntity<int>
{
    /// <summary>
    /// Name of the user group.
    /// </summary>
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Description of the user group.
    /// </summary>
    [MaxLength(2000)]
    public string? Description { get; set; }

    /// <summary>
    /// The profiles in this group.
    /// </summary>
    public IList<UserProfile>? Profiles { get; set; }
}
