using PortalForgeX.Shared.Features.Tenants;
using System.Text.Json.Serialization;

namespace PortalForgeX.Shared.Features.UserGroups;

public record UserGroupDto
{
    public int Id { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? LastModificationTime { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    [JsonIgnore]
    public IList<TenantUserViewModel>? Profiles { get; set; }
}
