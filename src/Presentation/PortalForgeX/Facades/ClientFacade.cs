using MediatR;
using PortalForgeX.Shared.Facades;
using PortalForgeX.Shared.Features.Clients;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Clients;

namespace PortalForgeX.Facades;

/// <inheritdoc/>
public class ClientFacade(ISender sender) : IClientFacade
{
    private const int DEFAULT_PAGE_SIZE = 25;

    /// <inheritdoc/>
    public async Task<GetClientsResponse?> GetAsync(
        int? pageIndex = null,
        int? pageSize = null,
        string? sortField = null,
        bool? sortAsc = null,
        string? filters = null,
        string? projectionFields = null,
        CancellationToken cancellationToken = default)
        => await sender.Send(new GetClientsRequest(new EntityPageSetting(
                pageSize ?? DEFAULT_PAGE_SIZE,
                pageIndex,
                sortField,
                sortAsc,
                filters,
                projectionFields)
            ), cancellationToken);

    /// <inheritdoc/>
    public async Task<GetClientByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await sender.Send(new GetClientByIdRequest(id), cancellationToken);

    /// <inheritdoc/>
    public async Task<CreateClientResponse?> CreateAsync(ClientDto model, CancellationToken cancellationToken = default)
        => await sender.Send(new CreateClientRequest(model), cancellationToken);

    /// <inheritdoc/>
    public async Task<UpdateClientResponse?> UpdateAsync(Guid id, ClientDto model, CancellationToken cancellationToken = default)
        => await sender.Send(new UpdateClientRequest(id, model), cancellationToken);

    /// <inheritdoc/>
    public async Task<DeleteClientResponse?> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        => await sender.Send(new DeleteClientRequest(id), cancellationToken);
}
