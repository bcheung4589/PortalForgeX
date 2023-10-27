using Microsoft.AspNetCore.Mvc;
using PortalForgeX.Endpoints.Internal;
using PortalForgeX.Extensions;
using PortalForgeX.Shared;
using PortalForgeX.Shared.Facades;
using PortalForgeX.Shared.Features.BusinessLocations;

namespace PortalForgeX.Endpoints;

public class GetBusinessLocationsEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapGet(BuildEndpointPath("businesslocations"), async (
            [FromHeader] int? pageIndex,
            [FromHeader] int? pageSize,
            [FromHeader] string? sortField,
            [FromHeader] bool? sortAsc,
            [FromHeader] string? filters,
            [FromHeader] string? projectionFields,
            IBusinessLocationFacade businessLocations,
            CancellationToken cancellationToken)
                => (await businessLocations.GetAsync(pageIndex, pageSize, sortField, sortAsc, filters, projectionFields, cancellationToken))?.ToResponse()
        ).WithTags("Business Locations");
}

public class GetBusinessLocationByIdEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapGet(BuildEndpointPath("businesslocation/{id:int}"), async (int id, IBusinessLocationFacade businessLocations, CancellationToken cancellationToken)
            => (await businessLocations.GetByIdAsync(id, cancellationToken))?.ToResponse()
        ).WithTags("Business Locations");
}

public class CreateBusinessLocationEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapPost(BuildEndpointPath("businesslocation"), async ([FromBody] BusinessLocationDto businessLocation, IBusinessLocationFacade businessLocations, CancellationToken cancellationToken)
            => (await businessLocations.CreateAsync(businessLocation, cancellationToken))?.ToResponse()
        ).WithTags("Business Locations");
}

public class UpdateBusinessLocationEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapPut(BuildEndpointPath("businesslocation/{id:int}"), async ([FromBody] BusinessLocationDto businessLocation, int id, IBusinessLocationFacade businessLocations, CancellationToken cancellationToken)
            => (await businessLocations.UpdateAsync(id, businessLocation, cancellationToken))?.ToResponse()
        ).WithTags("Business Locations");
}

public class DeleteBusinessLocationEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapDelete(BuildEndpointPath("businesslocation/{id:int}"), async (int id, IBusinessLocationFacade businessLocations, CancellationToken cancellationToken)
            => (await businessLocations.DeleteAsync(id, cancellationToken))?.ToResponse()
        ).WithTags("Business Locations");
}
