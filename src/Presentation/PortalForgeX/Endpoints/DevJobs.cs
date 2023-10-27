using MediatR;
using PortalForgeX.Endpoints.Internal;
using PortalForgeX.Extensions;
using PortalForgeX.Shared;
using PortalForgeX.Application.Features.DevJobs;

namespace PortalForgeX.Endpoints;

public class GenerateClientsEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapGet(BuildEndpointPath("devjobs/generate-clients/{amount:int?}"), async (int? amount, ISender sender, CancellationToken cancellationToken)
            => (await sender.Send(new GenerateClientsRequest(amount), cancellationToken)).ToResponse()
        ).WithTags("DevJobs");
}

public class GeneratePaymentsEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapGet(BuildEndpointPath("devjobs/generate-payments/{amount:int?}"), async (int? amount, ISender sender, CancellationToken cancellationToken)
            => (await sender.Send(new GeneratePaymentsRequest(amount), cancellationToken)).ToResponse()
        ).WithTags("DevJobs");
}
