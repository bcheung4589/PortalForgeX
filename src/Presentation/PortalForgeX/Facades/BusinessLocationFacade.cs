using MediatR;
using PortalForgeX.Shared.Facades;
using PortalForgeX.Shared.Features.BusinessLocations;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.BusinessLocations;

namespace PortalForgeX.Facades;

/// <inheritdoc/>
public class BusinessLocationFacade(ISender sender) : IBusinessLocationFacade
{
    private const int DEFAULT_PAGE_SIZE = 25;

    /// <inheritdoc/>
    public async Task<GetBusinessLocationsResponse?> GetAsync(int? pageIndex = null, int? pageSize = null, string? sortField = null, bool? sortAsc = null, string? filters = null, string? projectionFields = null, CancellationToken cancellationToken = default)
        => await sender.Send(new GetBusinessLocationsRequest(new EntityPageSetting(
                pageSize ?? DEFAULT_PAGE_SIZE,
                pageIndex,
                sortField,
                sortAsc,
                filters,
                projectionFields)
            ), cancellationToken);

    /// <inheritdoc/>
    public async Task<GetBusinessLocationByIdResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await sender.Send(new GetBusinessLocationByIdRequest(id), cancellationToken);

    /// <inheritdoc/>
    public async Task<CreateBusinessLocationResponse?> CreateAsync(BusinessLocationDto model, CancellationToken cancellationToken = default)
        => await sender.Send(new CreateBusinessLocationRequest(model), cancellationToken);

    /// <inheritdoc/>
    public async Task<UpdateBusinessLocationResponse?> UpdateAsync(int id, BusinessLocationDto model, CancellationToken cancellationToken = default)
        => await sender.Send(new UpdateBusinessLocationRequest(id, model), cancellationToken);

    /// <inheritdoc/>
    public async Task<DeleteBusinessLocationResponse?> DeleteAsync(int id, CancellationToken cancellationToken = default)
        => await sender.Send(new DeleteBusinessLocationRequest(id), cancellationToken);
}
