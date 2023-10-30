using PortalForgeX.Domain.Entities.Internal;
using System.ComponentModel.DataAnnotations;

namespace PortalForgeX.Domain.Entities.Tenants;

/// <summary>
/// User Profile of a Application User within a Tenant instance. The UserId is the primary key 
/// and MUST be the same Id of Identity.ApplicationUser.
/// 
/// Application Users can have Profiles in multiple Tenant instances.
/// </summary>
public class TenantUserProfile : IEntity
{
    /// <summary>
    /// User reference.
    /// </summary>
    public string UserId { get; set; } = null!;

    /// <summary>
    /// Title of the user.
    /// </summary>
    [MaxLength(100)]
    public string? Title { get; set; }

    /// <summary>
    /// The groups the User is in.
    /// </summary>
    public IList<UserGroup>? Groups { get; set; }

    /// <summary>
    /// UserProfile has a custom primary key.
    /// </summary>
    /// <returns>false</returns>
    public bool IsTransient() => false;
}
