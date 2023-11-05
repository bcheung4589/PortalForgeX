namespace PortalForgeX.Shared.Constants;

public static class SystemRolesNames
{
    /// <summary>
    /// Portal Administrator.
    /// </summary>
    public const string ADMIN = "Admin";

    /// <summary>
    /// Tenant Managers are responsible for Tenants.
    /// </summary>
    public const string MANAGER = "Manager";

    /// <summary>
    /// Tenant with Administrator role within Tenant application.
    /// </summary>
    public const string TENANT_ADMIN = "Tenant Admin";

    /// <summary>
    /// Tenant with User role within Tenant application.
    /// </summary>
    public const string TENANT_USER = "Tenant User";

    public static IEnumerable<string> ToList() => new[] { ADMIN, MANAGER, TENANT_ADMIN, TENANT_USER };
}
