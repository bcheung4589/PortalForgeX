using Microsoft.AspNetCore.Mvc;
using PortalForgeX.Endpoints.Internal;
using PortalForgeX.Extensions;
using PortalForgeX.Shared;
using PortalForgeX.Shared.Facades;
using PortalForgeX.Shared.Features.ClientContacts;

namespace PortalForgeX.Endpoints;

public class GetClientContactByIdEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapGet(BuildEndpointPath("clientcontact/{id:Guid}"), async (Guid id, IClientContactFacade contacts, CancellationToken cancellationToken)
            => (await contacts.GetByIdAsync(id, cancellationToken))?.ToResponse()
        ).WithTags("Client Contacts");
}

public class CreateClientContactEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapPost(BuildEndpointPath("clientcontact"), async ([FromBody] ClientContactDto client, IClientContactFacade contacts, CancellationToken cancellationToken)
            => (await contacts.CreateAsync(client, cancellationToken))?.ToResponse()
        ).WithTags("Client Contacts");
}

public class UpdateClientContactEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapPut(BuildEndpointPath("clientcontact/{id:Guid}"), async ([FromBody] ClientContactDto clientContact, Guid id, IClientContactFacade contacts, CancellationToken cancellationToken)
            => (await contacts.UpdateAsync(id, clientContact, cancellationToken))?.ToResponse()
        ).WithTags("Client Contacts");
}

public class DeleteClientContactEndpoint : ApiEndpoint_v1, IFeatureEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
        => app.MapDelete(BuildEndpointPath("clientcontact/{id:Guid}"), async (Guid id, IClientContactFacade contacts, CancellationToken cancellationToken)
            => (await contacts.DeleteAsync(id, cancellationToken))?.ToResponse()
        ).WithTags("Client Contacts");
}
