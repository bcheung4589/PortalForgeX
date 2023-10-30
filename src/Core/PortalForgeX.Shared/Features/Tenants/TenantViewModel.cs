namespace PortalForgeX.Shared.Features.Tenants;

/// <summary>
/// Aggregated ViewModel for <see cref="PortalForgeX.Domain.Entities.Tenants.Tenant"/> 
/// and <see cref="PortalForgeX.Domain.Entities.Tenants.TenantSettings"/>.
/// </summary>
public record TenantViewModel
{
    /*
     * Tenant
     */

    public Guid Id { get; set; }
    public string ExternalId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string InternalName { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string Host { get; set; } = null!;
    public bool IsActive { get; set; }
    public string? Remarks { get; set; }

    /*
     * TenantSettings
     */

    public string? LogoUrl { get; set; }
    public string? Brand { get; set; }
    public string? PrimaryColor { get; set; }
    public string? SecondaryColor { get; set; }
    public bool IsPublicRegisterEnabled { get; set; }
    public string? AdditionalData { get; set; }

    /*
     * ApplicationUser
     */

    public string? ManagerId { get; set; }
    public string? ManagerName { get; set; }

    public IList<TenantUserViewModel>? Users { get; set; }
}
