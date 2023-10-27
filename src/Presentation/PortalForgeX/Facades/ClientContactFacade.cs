using MediatR;
using PortalForgeX.Shared.Facades;
using PortalForgeX.Shared.Features.ClientContacts;
using PortalForgeX.Application.Features.ClientContacts;

namespace PortalForgeX.Facades;

/// <inheritdoc/>
public class ClientContactFacade(ISender sender) : IClientContactFacade
{
    /// <inheritdoc/>
    public async Task<GetClientContactByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await sender.Send(new GetClientContactByIdRequest(id), cancellationToken);

    /// <inheritdoc/>
    public async Task<CreateClientContactResponse?> CreateAsync(ClientContactDto model, CancellationToken cancellationToken = default)
        => await sender.Send(new CreateClientContactRequest(model), cancellationToken);

    /// <inheritdoc/>
    public async Task<UpdateClientContactResponse?> UpdateAsync(Guid id, ClientContactDto model, CancellationToken cancellationToken = default)
        => await sender.Send(new UpdateClientContactRequest(id, model), cancellationToken);

    /// <inheritdoc/>
    public async Task<DeleteClientContactResponse?> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        => await sender.Send(new DeleteClientContactRequest(id), cancellationToken);
}
