using PortalForgeX.Shared.Features.ClientContacts;

namespace PortalForgeX.Shared.Facades;

/// <summary>
/// The IClientContactFacade exposes all the features concerning client contacts entities.
/// </summary>
public interface IClientContactFacade : IAppFeatureFacade
{
    /// <summary>
    /// Get client contact by id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetClientContactByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create client contact.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CreateClientContactResponse?> CreateAsync(ClientContactDto model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update client contact.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<UpdateClientContactResponse?> UpdateAsync(Guid id, ClientContactDto model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete client contact.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DeleteClientContactResponse?> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
