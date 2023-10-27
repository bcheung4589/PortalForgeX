using PortalForgeX.Shared.Features.Clients;

namespace PortalForgeX.Shared.Facades;

/// <summary>
/// The IClientFacade exposes all the features concerning clients entities.
/// </summary>
public interface IClientFacade : IAppFeatureFacade
{
    /// <summary>
    /// Get the clients.
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortField"></param>
    /// <param name="sortAsc"></param>
    /// <param name="filters"></param>
    /// <param name="projectionFields"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetClientsResponse?> GetAsync(
        int? pageIndex = null,
        int? pageSize = null,
        string? sortField = null,
        bool? sortAsc = null,
        string? filters = null,
        string? projectionFields = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get client by id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetClientByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create client.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CreateClientResponse?> CreateAsync(ClientDto model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update client.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<UpdateClientResponse?> UpdateAsync(Guid id, ClientDto model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete client.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DeleteClientResponse?> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
