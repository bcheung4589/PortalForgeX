using MediatR;
using Microsoft.AspNetCore.Mvc;
using PortalForgeX.Endpoints.Internal;
using PortalForgeX.Extensions;
using PortalForgeX.Shared;
using PortalForgeX.Shared.Features.Payments;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Payments;

namespace PortalForgeX.Endpoints;

public class GetPaymentsEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    private const int DEFAULT_PAGE_SIZE = 25;

    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapGet(BuildEndpointPath("payments"), async (
            [FromHeader] int? pageIndex,
            [FromHeader] int? pageSize,
            [FromHeader] string? sortField,
            [FromHeader] bool? sortAsc,
            [FromHeader] string? filters,
            ISender sender,
            CancellationToken cancellationToken)
                => (await sender.Send(new GetPaymentsRequest(new EntityPageSetting(pageSize ?? DEFAULT_PAGE_SIZE, pageIndex, sortField, sortAsc, filters)), cancellationToken)).ToResponse()
        ).WithTags("Payments");
}

public class GetPaymentByIdEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapGet(BuildEndpointPath("payment/{id:Guid}"), async (Guid id, ISender sender, CancellationToken cancellationToken)
            => (await sender.Send(new GetPaymentByIdRequest(id), cancellationToken)).ToResponse()
        ).WithTags("Payments");
}

public class CreatePaymentEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapPost(BuildEndpointPath("payment"), async ([FromBody] PaymentDto payment, ISender sender, CancellationToken cancellationToken)
            => (await sender.Send(new CreatePaymentRequest(payment), cancellationToken)).ToResponse()
        ).WithTags("Payments");
}

public class UpdatePaymentEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapPut(BuildEndpointPath("payment/{id:Guid}"), async ([FromBody] PaymentDto payment, Guid id, ISender sender, CancellationToken cancellationToken)
            => (await sender.Send(new UpdatePaymentRequest(id, payment), cancellationToken)).ToResponse()
        ).WithTags("Payments");
}

public class DeletePaymentEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapDelete(BuildEndpointPath("payment/{id:Guid}"), async (Guid id, ISender sender, CancellationToken cancellationToken)
            => (await sender.Send(new DeletePaymentRequest(id), cancellationToken)).ToResponse()
        ).WithTags("Payments");
}
