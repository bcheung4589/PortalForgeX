using Blazored.Toast.Services;
using PortalForgeX.Client.Extensions;
using PortalForgeX.Shared;
using PortalForgeX.Shared.Facades;
using PortalForgeX.Shared.Features.Clients;
using System.Net.Http.Json;

namespace PortalForgeX.Client.Facades;

/// <summary>
/// The HttpClientFacade exposes all the API features 
/// available concerning client entities.
/// </summary>
/// <param name="httpClientFactory"></param>
/// <param name="toastService"></param>
public class HttpClientFacade(IHttpClientFactory httpClientFactory, IToastService toastService) : IClientFacade
{
    private readonly HttpClient http = httpClientFactory.CreateServerClient();

    /// <inheritdoc/>
    public async Task<GetClientsResponse?> GetAsync(
        int? pageIndex = null,
        int? pageSize = null,
        string? sortField = null,
        bool? sortAsc = null,
        string? filters = null,
        string? projectionFields = null,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoint_v1.BuildEndpointPath("clients"))
            .PopulateHeadersWith(new Dictionary<string, string?>
            {
                { "pageIndex", pageIndex.ToString() },
                { "pageSize", pageSize.ToString() },
                { "sortField", sortField },
                { "sortAsc", sortAsc.ToString() },
                { "filters", filters },
                { "projectionFields", projectionFields }
            });

        return await request.Execute<GetClientsResponse>(http, toastService: toastService, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<GetClientByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoint_v1.BuildEndpointPath($"client/{id}"));

        return await request.Execute<GetClientByIdResponse>(http, toastService: toastService, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<CreateClientResponse?> CreateAsync(ClientDto model, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, ApiEndpoint_v1.BuildEndpointPath("client"))
        {
            Content = JsonContent.Create(model)
        };

        return await request.Execute<CreateClientResponse>(http, toastService: toastService, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<UpdateClientResponse?> UpdateAsync(Guid id, ClientDto model, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, ApiEndpoint_v1.BuildEndpointPath($"client/{id}"))
        {
            Content = JsonContent.Create(model)
        };

        return await request.Execute<UpdateClientResponse>(http, toastService: toastService, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<DeleteClientResponse?> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, ApiEndpoint_v1.BuildEndpointPath($"client/{id}"));

        return await request.Execute<DeleteClientResponse>(http, toastService: toastService, cancellationToken: cancellationToken);
    }
}
