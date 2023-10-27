namespace PortalForgeX.Shared.Features.ClientContacts;

public record ClientContactDto
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? LastModificationTime { get; set; }

    public string FullName { get; set; } = null!;
    public string PhoneNr { get; set; } = null!;
    public string? Email { get; set; }
    public bool IsActive { get; set; }
    public string? Remarks { get; set; }

    public Guid ClientId { get; set; }
}
