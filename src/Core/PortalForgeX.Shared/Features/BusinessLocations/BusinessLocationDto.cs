using System.Text.Json.Serialization;
using PortalForgeX.Shared.Features.Checkouts;
using PortalForgeX.Shared.Features.Clients;
using PortalForgeX.Shared.Features.Payments;

namespace PortalForgeX.Shared.Features.BusinessLocations;

public record BusinessLocationDto
{
    public int Id { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? LastModificationTime { get; set; }

    public string ApiKey { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string HouseNr { get; set; } = null!;
    public string Zipcode { get; set; } = null!;
    public string Place { get; set; } = null!;
    public string Country { get; set; } = null!;
    public bool HasSubscription { get; set; }
    public bool IsActive { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? IpAddress { get; set; }
    public string? Remarks { get; set; }

    [JsonIgnore]
    public ClientDto? Client { get; set; }
    public Guid ClientId { get; set; }

    public IEnumerable<CheckoutDto>? Checkouts { get; set; }

    [JsonIgnore]
    public IEnumerable<PaymentDto>? Payments { get; set; }
}
