using PortalForgeX.Domain.Entities.Identity;
using PortalForgeX.Domain.Entities.Internal;
using PortalForgeX.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PortalForgeX.Domain.Entities.Tenants;

public class Tenant : AuditedEntity<Guid>
{
    /// <summary>
    /// External ID that can be used for public communication.
    /// </summary>
    [MaxLength(50)]
    public string ExternalId { get; set; } = null!;

    /// <summary>
    /// Name of the tenant/organisation.
    /// </summary>
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Internal name is used for naming system resources bound to this tenant.
    /// E.g. database name.
    /// </summary>
    [MaxLength(100)]
    public string InternalName { get; set; } = null!;

    /// <summary>
    /// The Status of the Tenant.
    /// </summary>
    public TenantStatus Status { get; set; }

    /// <summary>
    /// Host(domain) name of the tenant.
    /// </summary>
    [MaxLength(150)]
    public string Host { get; set; } = null!;

    /// <summary>
    /// Indicator if the tenant is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Remarks about the tenant.
    /// </summary>
    [MaxLength(2000)]
    public string? Remarks { get; set; }

    /** 
	 * relations 
	 */

    /// <summary>
    /// Settings Reference.
    /// </summary>
    public Guid TenantSettingsId { get; set; }

    /// <summary>
    /// The settings for the tenant.
    /// </summary>
    public TenantSettings TenantSettings { get; set; } = null!;

    /// <summary>
    /// Application User Id from the Tenant Manager.
    /// </summary>
    public string? ManagerId { get; set; }

    /// <summary>
    /// The Manager for this Tenant.
    /// </summary>
    public ApplicationUser? Manager { get; set; }
}
