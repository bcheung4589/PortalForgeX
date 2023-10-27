namespace PortalForgeX.Domain.Enums;

/// <summary>
/// The different states the Tenants goes through from creation to ready for usage.
/// </summary>
public enum TenantStatus
{
    /// <summary>
    /// Tenant has been created and pending approval.
    /// </summary>
    Created = 0,

    /// <summary>
    /// Rejected path.
    /// </summary>
    Rejected = -1,

    /// <summary>
    /// Tenant has been Approved and is ready for database migration.
    /// </summary>
    Approved = 10,

    /// <summary>
    /// Tenant database has been migrated.
    /// </summary>
    DbMigrated = 20,

    /// <summary>
    /// Tenant application is ready to be used.
    /// </summary>
    Ready = 100
}
