namespace PortalForgeX.Client.Authentication;

public class UserContext
{
    public required string UserId { get; set; }
    public required string Email { get; set; }

    public string? TenantId { get; set; }
}
