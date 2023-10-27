using PortalForgeX.Shared.Features.Checkouts;

namespace PortalForgeX.Shared.Facades;

/// <summary>
/// The ICheckoutFacade exposes all the features concerning checkouts entities.
/// </summary>
public interface ICheckoutFacade : IAppFeatureFacade
{
    /// <summary>
    /// Get checkout by id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetCheckoutByIdResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create checkout.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CreateCheckoutResponse?> CreateAsync(CheckoutDto model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update checkout.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<UpdateCheckoutResponse?> UpdateAsync(int id, CheckoutDto model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete checkout.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DeleteCheckoutResponse?> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
