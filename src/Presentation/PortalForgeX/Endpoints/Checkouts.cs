using MediatR;
using Microsoft.AspNetCore.Mvc;
using PortalForgeX.Endpoints.Internal;
using PortalForgeX.Extensions;
using PortalForgeX.Shared;
using PortalForgeX.Shared.Features.Checkouts;
using PortalForgeX.Application.Features.Checkouts;
using PortalForgeX.Domain.Entities;

namespace PortalForgeX.Endpoints;

public class GetCheckoutByIdEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapGet(BuildEndpointPath("checkout/{id:int}"), async (int id, ISender sender, CancellationToken cancellationToken)
            => (await sender.Send(new GetCheckoutByIdRequest(id), cancellationToken)).ToResponse()
        ).WithTags("Checkouts");
}

public class CreateCheckoutEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapPost(BuildEndpointPath("checkout"), async ([FromBody] CheckoutDto checkout, ISender sender, CancellationToken cancellationToken)
            => (await sender.Send(new CreateCheckoutRequest(checkout), cancellationToken)).ToResponse()
        ).WithTags("Checkouts");
}

public class UpdateCheckoutEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapPut(BuildEndpointPath("checkout/{id:int}"), async ([FromBody] CheckoutDto checkout, int id, ISender sender, CancellationToken cancellationToken)
            => (await sender.Send(new UpdateCheckoutRequest(id, checkout), cancellationToken)).ToResponse()
        ).WithTags("Checkouts");
}

public class DeleteCheckoutEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapDelete(BuildEndpointPath("checkout/{id:int}"), async (int id, ISender sender, CancellationToken cancellationToken)
            => (await sender.Send(new DeleteCheckoutRequest(id), cancellationToken)).ToResponse()
        ).WithTags("Checkouts");
}
