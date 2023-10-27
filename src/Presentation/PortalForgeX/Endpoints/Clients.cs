using Microsoft.AspNetCore.Mvc;
using PortalForgeX.Endpoints.Internal;
using PortalForgeX.Extensions;
using PortalForgeX.Filters;
using PortalForgeX.Infrastructure.FeatureManagement;
using PortalForgeX.Shared;
using PortalForgeX.Shared.Facades;
using PortalForgeX.Shared.Features.Clients;

namespace PortalForgeX.Endpoints;

public class GetClientsEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public string FeatureName => nameof(FeatureFlags.GetClients);

    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapGet(BuildEndpointPath("clients"), async (
            [FromHeader] int? pageIndex,
            [FromHeader] int? pageSize,
            [FromHeader] string? sortField,
            [FromHeader] bool? sortAsc,
            [FromHeader] string? filters,
            [FromHeader] string? projectionFields,
            IClientFacade clients,
            CancellationToken cancellationToken)
                => (await clients.GetAsync(pageIndex, pageSize, sortField, sortAsc, filters, projectionFields, cancellationToken))?.ToResponse()
        ).WithTags("Clients")
        .AddEndpointFilter(async (context, next) => await next(FeatureFilter.SetRequestingFeature(context, FeatureName)))
        .AddEndpointFilter<FeatureFilter>();
}

public class GetClientByIdEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public string FeatureName => nameof(FeatureFlags.GetClientById);

    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapGet(BuildEndpointPath("client/{id:Guid}"), async (Guid id, IClientFacade clients, CancellationToken cancellationToken)
            => (await clients.GetByIdAsync(id, cancellationToken))?.ToResponse()
        ).WithTags("Clients")
        .AddEndpointFilter(async (context, next) => await next(FeatureFilter.SetRequestingFeature(context, FeatureName)))
        .AddEndpointFilter<FeatureFilter>();
}

public class CreateClientEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public string FeatureName => nameof(FeatureFlags.CreateClient);

    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapPost(BuildEndpointPath("client"), async ([FromBody] ClientDto client, IClientFacade clients, CancellationToken cancellationToken)
            => (await clients.CreateAsync(client, cancellationToken))?.ToResponse()
        ).WithTags("Clients")
        .AddEndpointFilter<ValidationFilter<ClientDto>>()
        .AddEndpointFilter(async (context, next) => await next(FeatureFilter.SetRequestingFeature(context, FeatureName)))
        .AddEndpointFilter<FeatureFilter>();
}

public class UpdateClientEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public string FeatureName => nameof(FeatureFlags.UpdateClient);

    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapPut(BuildEndpointPath("client/{id:Guid}"), async ([FromBody] ClientDto client, Guid id, IClientFacade clients, CancellationToken cancellationToken)
            => (await clients.UpdateAsync(id, client, cancellationToken))?.ToResponse()
        ).WithTags("Clients")
        .AddEndpointFilter<ValidationFilter<ClientDto>>()
        .AddEndpointFilter(async (context, next) => await next(FeatureFilter.SetRequestingFeature(context, FeatureName)))
        .AddEndpointFilter<FeatureFilter>();
}

public class DeleteClientEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public string FeatureName => nameof(FeatureFlags.DeleteClient);

    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapDelete(BuildEndpointPath("client/{id:Guid}"), async (Guid id, IClientFacade clients, CancellationToken cancellationToken)
            => (await clients.DeleteAsync(id, cancellationToken))?.ToResponse()
        ).WithTags("Clients")
        .AddEndpointFilter(async (context, next) => await next(FeatureFilter.SetRequestingFeature(context, FeatureName)))
        .AddEndpointFilter<FeatureFilter>();
}
