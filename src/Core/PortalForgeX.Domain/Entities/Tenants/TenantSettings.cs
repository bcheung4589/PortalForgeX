using PortalForgeX.Domain.Entities.Internal;
using System.ComponentModel.DataAnnotations;

namespace PortalForgeX.Domain.Entities.Tenants;

public class TenantSettings : AuditedEntity<Guid>
{
    /// <summary>
    /// Full url to the logo.
    /// </summary>
    [MaxLength(250)]
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Brand name of the tenant.
    /// </summary>
    [MaxLength(150)]
    public string? Brand { get; set; }

    /// <summary>
    /// Primairy coloring for themes.
    /// </summary>
    [MaxLength(50)]
    public string? PrimaryColor { get; set; }

    /// <summary>
    /// Secondary coloring for themes.
    /// </summary>
    [MaxLength(50)]
    public string? SecondaryColor { get; set; }

    /// <summary>
    /// Setting for enabling/disabling the public registeration form.
    /// </summary>
    public bool IsPublicRegisterEnabled { get; set; }

    /// <summary>
    /// Additional Data can be saved in JSON format.
    /// </summary>
    public string? AdditionalData { get; set; }
}
