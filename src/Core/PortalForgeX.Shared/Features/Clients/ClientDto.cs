using PortalForgeX.Shared.Features.BusinessLocations;
using PortalForgeX.Shared.Features.ClientContacts;
using PortalForgeX.Shared.Features.Payments;
using System.Text.Json.Serialization;

namespace PortalForgeX.Shared.Features.Clients;

public record ClientDto
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? LastModificationTime { get; set; }

    public string Name { get; set; } = null!;
    public bool HasCustomerCarePlus { get; set; }
    public bool IsActive { get; set; }
    public string? Remarks { get; set; }

    public IEnumerable<ClientContactDto>? Contacts { get; set; }
    public IEnumerable<BusinessLocationDto>? Locations { get; set; }

    [JsonIgnore]
    public IEnumerable<PaymentDto>? Payments { get; set; }
}