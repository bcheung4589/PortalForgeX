using Microsoft.AspNetCore.Identity;
using PortalForgeX.Domain.Enums;

namespace PortalForgeX.Domain.Entities.Identity;

public class ApplicationRole : IdentityRole
{
    /// <summary>
    /// Description of the role.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The type of this role.
    /// </summary>
    public RoleType RoleType { get; set; }

    public ApplicationRole() : base() { }

    public ApplicationRole(string roleName) : base(roleName) { }

    public ApplicationRole(string roleName, RoleType roleType) : base(roleName)
    {
        RoleType = roleType;
    }
}
