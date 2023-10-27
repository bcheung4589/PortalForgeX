using System.Text.Json.Serialization;
using PortalForgeX.Shared.Features.BusinessLocations;
using PortalForgeX.Shared.Features.Clients;

namespace PortalForgeX.Shared.Features.Payments;

public record PaymentDto
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? LastModificationTime { get; set; }

    public string TransactionId { get; set; } = null!;
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = null!;
    public DateTime PaymentPeriod { get; set; }
    public DateTime FulfilledDate { get; set; }
    public string? Remarks { get; set; }

    public ClientDto Client { get; set; } = null!;
    public Guid ClientId { get; set; }

    public BusinessLocationDto? BusinessLocation { get; set; }
    public int? BusinessLocationId { get; set; }
}
