using PortalForgeX.Domain.Entities.Internal;
using System.ComponentModel.DataAnnotations;

namespace PortalForgeX.Domain.Entities;

/// <summary>
/// Profile of a Tenant User. The UserId is the primary key 
/// and MUST be the same Id of Identity.ApplicationUser.
/// 
/// ApplicationUsers can have Profiles in multiple Tenant instances.
/// </summary>
public class UserProfile : IEntity
{
    /// <summary>
    /// User reference.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Firstname of the user.
    /// </summary>
    [MaxLength(100)]
    public string? FirstName { get; set; }

    /// <summary>
    /// Lastname of the user.
    /// </summary>
    [MaxLength(100)]
    public string? LastName { get; set; }

    /// <summary>
    /// The time the profile was created.
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// Last time this profile was accessed.
    /// </summary>
    public DateTime? LastAccessTime { get; set; }

    /// <summary>
    /// Last time the profile was modified.
    /// </summary>
    public DateTime? LastModificationTime { get; set; }

    /// <summary>
    /// The groups the User is in.
    /// </summary>
    public IList<UserGroup>? Groups { get; set; }

    /// <summary>
    /// UserProfile has a composite primary key.
    /// </summary>
    /// <returns>false</returns>
    public bool IsTransient() => false;
}
