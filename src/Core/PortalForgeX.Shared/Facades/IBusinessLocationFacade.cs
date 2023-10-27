using PortalForgeX.Shared.Features.BusinessLocations;

namespace PortalForgeX.Shared.Facades;

/// <summary>
/// The IBusinessLocationFacade exposes all the features concerning client contacts entities.
/// </summary>
public interface IBusinessLocationFacade : IAppFeatureFacade
{
    /// <summary>
    /// Get the business locations.
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortField"></param>
    /// <param name="sortAsc"></param>
    /// <param name="filters"></param>
    /// <param name="projectionFields"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetBusinessLocationsResponse?> GetAsync(
        int? pageIndex = null,
        int? pageSize = null,
        string? sortField = null,
        bool? sortAsc = null,
        string? filters = null,
        string? projectionFields = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get business location by id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetBusinessLocationByIdResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create business location.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CreateBusinessLocationResponse?> CreateAsync(BusinessLocationDto model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update business location.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<UpdateBusinessLocationResponse?> UpdateAsync(int id, BusinessLocationDto model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete business location.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DeleteBusinessLocationResponse?> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
