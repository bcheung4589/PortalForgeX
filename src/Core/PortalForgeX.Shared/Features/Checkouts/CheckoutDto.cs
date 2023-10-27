using System.Text.Json.Serialization;
using PortalForgeX.Shared.Features.BusinessLocations;

namespace PortalForgeX.Shared.Features.Checkouts;

public record CheckoutDto
{
    public int Id { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? LastModificationTime { get; set; }

    public string DeviceCode { get; set; } = null!;
    public string SoftwareVersion { get; set; } = null!;
    public bool IsActive { get; set; }
    public string? Remarks { get; set; }

    [JsonIgnore]
    public BusinessLocationDto? BusinessLocation { get; set; }
    public int BusinessLocationId { get; set; }
}
