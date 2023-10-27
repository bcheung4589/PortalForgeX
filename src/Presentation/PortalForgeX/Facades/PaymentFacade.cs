using MediatR;
using PortalForgeX.Shared.Facades;
using PortalForgeX.Shared.Features.Payments;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Payments;

namespace PortalForgeX.Facades;

/// <inheritdoc/>
public class PaymentFacade(ISender sender) : IPaymentFacade
{
    private const int DEFAULT_PAGE_SIZE = 25;

    /// <inheritdoc/>
    public async Task<GetPaymentsResponse?> GetAsync(int? pageIndex = null, int? pageSize = null, string? sortField = null, bool? sortAsc = null, string? filters = null, string? projectionFields = null, CancellationToken cancellationToken = default)
                => await sender.Send(new GetPaymentsRequest(new EntityPageSetting(
                pageSize ?? DEFAULT_PAGE_SIZE,
                pageIndex,
                sortField,
                sortAsc,
                filters,
                projectionFields)
            ), cancellationToken);

    /// <inheritdoc/>
    public async Task<GetPaymentByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await sender.Send(new GetPaymentByIdRequest(id), cancellationToken);

    /// <inheritdoc/>
    public async Task<CreatePaymentResponse?> CreateAsync(PaymentDto model, CancellationToken cancellationToken = default)
        => await sender.Send(new CreatePaymentRequest(model), cancellationToken);

    /// <inheritdoc/>
    public async Task<UpdatePaymentResponse?> UpdateAsync(Guid id, PaymentDto model, CancellationToken cancellationToken = default)
        => await sender.Send(new UpdatePaymentRequest(id, model), cancellationToken);

    /// <inheritdoc/>
    public async Task<DeletePaymentResponse?> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        => await sender.Send(new DeletePaymentRequest(id), cancellationToken);
}
