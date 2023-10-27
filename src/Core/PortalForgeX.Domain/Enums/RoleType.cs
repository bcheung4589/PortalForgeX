namespace PortalForgeX.Domain.Enums;

public enum RoleType
{
    /// <summary>
    /// System roles are created and managed
    /// by the application and cant be deleted.
    /// 
    /// <see cref="CRM.Shared.Constants.SystemRolesNames"/>
    /// </summary>
    System = 0,

    /// <summary>
    /// User roles are created and managed
    /// by the Application Users.
    /// </summary>
    User = 100
}
