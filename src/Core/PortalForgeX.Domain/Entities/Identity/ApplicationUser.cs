using Microsoft.AspNetCore.Identity;
using PortalForgeX.Domain.Entities.Internal;
using PortalForgeX.Domain.Entities.Tenants;
using System.ComponentModel.DataAnnotations;

namespace PortalForgeX.Domain.Entities.Identity;

public class ApplicationUser : IdentityUser, IHasCreationTime, IHasModificationTime
{
    /// <inheritdoc />
    public DateTime CreationTime { get; set; }

    /// <inheritdoc />
    public DateTime? LastModificationTime { get; set; }

    /// <summary>
    /// Last time the user has successfully logged in.
    /// </summary>
    public DateTime? LastLoggedInTime { get; set; }

    /// <summary>
    /// Indicator if the user is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Remarks about the user.
    /// </summary>
    [MaxLength(2000)]
    public string? Remarks { get; set; }

    /** 
	 * relations 
	 */

    /// <summary>
    /// Reference Id to which tenant this user belongs. 
    /// If null, its a portal user.
    /// </summary>
    public Guid? TenantId { get; set; }

    /// <summary>
    /// Tenant instance.
    /// </summary>
    public Tenant? Tenant { get; set; }
}
