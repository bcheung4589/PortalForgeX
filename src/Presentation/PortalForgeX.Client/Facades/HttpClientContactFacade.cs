using Blazored.Toast.Services;
using PortalForgeX.Client.Extensions;
using PortalForgeX.Shared;
using PortalForgeX.Shared.Facades;
using PortalForgeX.Shared.Features.ClientContacts;
using PortalForgeX.Shared.Features.Clients;
using System.Net.Http.Json;

namespace PortalForgeX.Client.Facades;

/// <summary>
/// The HttpClientContactFacade exposes all the API features 
/// available concerning client contact entities.
/// </summary>
/// <param name="httpClientFactory"></param>
/// <param name="toastService"></param>
public class HttpClientContactFacade(IHttpClientFactory httpClientFactory, IToastService toastService) : IClientContactFacade
{
    private readonly HttpClient http = httpClientFactory.CreateServerClient();

    /// <inheritdoc/>
    public async Task<GetClientContactByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoint_v1.BuildEndpointPath($"clientcontact/{id}"));

        return await request.Execute<GetClientContactByIdResponse>(http, toastService: toastService, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<CreateClientContactResponse?> CreateAsync(ClientContactDto model, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, ApiEndpoint_v1.BuildEndpointPath("clientcontact"))
        {
            Content = JsonContent.Create(model)
        };

        return await request.Execute<CreateClientContactResponse>(http, toastService: toastService, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<UpdateClientContactResponse?> UpdateAsync(Guid id, ClientContactDto model, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, ApiEndpoint_v1.BuildEndpointPath($"clientcontact/{id}"))
        {
            Content = JsonContent.Create(model)
        };

        return await request.Execute<UpdateClientContactResponse>(http, toastService: toastService, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<DeleteClientContactResponse?> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, ApiEndpoint_v1.BuildEndpointPath($"clientcontact/{id}"));

        return await request.Execute<DeleteClientContactResponse>(http, toastService: toastService, cancellationToken: cancellationToken);
    }
}
