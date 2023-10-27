using MediatR;
using PortalForgeX.Shared.Facades;
using PortalForgeX.Shared.Features.Checkouts;
using PortalForgeX.Application.Features.Checkouts;

namespace PortalForgeX.Facades;

/// <inheritdoc/>
public class CheckoutFacade(ISender sender) : ICheckoutFacade
{
    /// <inheritdoc/>
    public async Task<GetCheckoutByIdResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await sender.Send(new GetCheckoutByIdRequest(id), cancellationToken);

    /// <inheritdoc/>
    public async Task<CreateCheckoutResponse?> CreateAsync(CheckoutDto model, CancellationToken cancellationToken = default)
        => await sender.Send(new CreateCheckoutRequest(model), cancellationToken);

    /// <inheritdoc/>
    public async Task<UpdateCheckoutResponse?> UpdateAsync(int id, CheckoutDto model, CancellationToken cancellationToken = default)
        => await sender.Send(new UpdateCheckoutRequest(id, model), cancellationToken);

    /// <inheritdoc/>
    public async Task<DeleteCheckoutResponse?> DeleteAsync(int id, CancellationToken cancellationToken = default)
        => await sender.Send(new DeleteCheckoutRequest(id), cancellationToken);
}
