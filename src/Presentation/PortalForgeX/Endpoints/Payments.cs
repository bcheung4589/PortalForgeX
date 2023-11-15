using Microsoft.AspNetCore.Mvc;
using PortalForgeX.Endpoints.Internal;
using PortalForgeX.Extensions;
using PortalForgeX.Shared;
using PortalForgeX.Shared.Facades;
using PortalForgeX.Shared.Features.Payments;

namespace PortalForgeX.Endpoints;

public class GetPaymentsEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapGet(BuildEndpointPath("payments"), async (
            [FromHeader] int? pageIndex,
            [FromHeader] int? pageSize,
            [FromHeader] string? sortField,
            [FromHeader] bool? sortAsc,
            [FromHeader] string? filters,
            IPaymentFacade payments,
            CancellationToken cancellationToken)
                => (await payments.GetAsync(pageIndex, pageSize, sortField, sortAsc, filters, cancellationToken: cancellationToken))?.ToResponse()
        ).WithTags("Payments");
}

public class GetPaymentByIdEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapGet(BuildEndpointPath("payment/{id:Guid}"), async (Guid id, IPaymentFacade payments, CancellationToken cancellationToken)
            => (await payments.GetByIdAsync(id, cancellationToken))?.ToResponse()
        ).WithTags("Payments");
}

public class CreatePaymentEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapPost(BuildEndpointPath("payment"), async ([FromBody] PaymentDto payment, IPaymentFacade payments, CancellationToken cancellationToken)
            => (await payments.CreateAsync(payment, cancellationToken))?.ToResponse()
        ).WithTags("Payments");
}

public class UpdatePaymentEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapPut(BuildEndpointPath("payment/{id:Guid}"), async ([FromBody] PaymentDto payment, Guid id, IPaymentFacade payments, CancellationToken cancellationToken)
            => (await payments.UpdateAsync(id, payment, cancellationToken))?.ToResponse()
        ).WithTags("Payments");
}

public class DeletePaymentEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapDelete(BuildEndpointPath("payment/{id:Guid}"), async (Guid id, IPaymentFacade payments, CancellationToken cancellationToken)
            => (await payments.DeleteAsync(id, cancellationToken))?.ToResponse()
        ).WithTags("Payments");
}
