namespace PortalForgeX.Shared.Features.Tenants;

public record TenantFormModel
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
    public DateTime CreationTime { get; set; }
    public DateTime? LastModificationTime { get; set; }

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
}
