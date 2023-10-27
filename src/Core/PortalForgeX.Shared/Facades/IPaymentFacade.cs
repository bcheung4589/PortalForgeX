using PortalForgeX.Shared.Features.Payments;

namespace PortalForgeX.Shared.Facades;

/// <summary>
/// The IPaymentFacade exposes all the features concerning clients entities.
/// </summary>
public interface IPaymentFacade : IAppFeatureFacade
{
    /// <summary>
    /// Get the payments.
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortField"></param>
    /// <param name="sortAsc"></param>
    /// <param name="filters"></param>
    /// <param name="projectionFields"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetPaymentsResponse?> GetAsync(
        int? pageIndex = null,
        int? pageSize = null,
        string? sortField = null,
        bool? sortAsc = null,
        string? filters = null,
        string? projectionFields = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get payment by id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetPaymentByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create payment.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CreatePaymentResponse?> CreateAsync(PaymentDto model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update payment.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<UpdatePaymentResponse?> UpdateAsync(Guid id, PaymentDto model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete payment.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DeletePaymentResponse?> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
