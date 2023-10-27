using PortalForgeX.Shared.DTOs;

namespace PortalForgeX.Shared.Features.Clients;

public record GetClientsResponse : Result<PageModel<ClientDto>>;

public record GetClientByIdResponse : Result<ClientDto>;

public record CreateClientResponse : Result<ClientDto>;

public record GenerateClientsResponse : Result<int>;

public record UpdateClientResponse : Result<ClientDto>;

public record DeleteClientResponse : Result;
