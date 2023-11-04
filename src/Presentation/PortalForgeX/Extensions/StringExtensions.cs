using PortalForgeX.Domain.Enums;

namespace PortalForgeX.Extensions;

public static partial class StringExtensions
{
    public static TenantStatus ToTenantStatus(this string source)
    {
        return Enum.TryParse<TenantStatus>(source, out var status)
            ? status
            : TenantStatus.Created;
    }
}
