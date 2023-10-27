namespace PortalForgeX.Shared.Features.ClientContacts;

public record GetClientContactByIdResponse : Result<ClientContactDto>;

public record CreateClientContactResponse : Result<ClientContactDto>;

public record UpdateClientContactResponse : Result<ClientContactDto>;

public record DeleteClientContactResponse : Result;