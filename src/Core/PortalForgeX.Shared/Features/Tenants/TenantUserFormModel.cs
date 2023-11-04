namespace PortalForgeX.Shared.Features.Tenants;

public record TenantUserFormModel
{
    /*
     * ApplicationUser
     */

    public string Id { get; set; } = default!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public virtual string? UserName { get; set; }
    public virtual string? Email { get; set; }
    public virtual string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public string? Remarks { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? LastModificationTime { get; set; }
    public DateTime? LastLoggedInTime { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public int AccessFailedCount { get; set; }
    public bool LockoutEnabled { get; set; }

    public Guid TenantId { get; set; }

    /*
     * TenantUserProfile
     */
    public string? Title { get; set; }
}
