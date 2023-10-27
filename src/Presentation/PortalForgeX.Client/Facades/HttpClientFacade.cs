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
        var requestStartTime = DateTime.UtcNow;
        var requestUri = ApiEndpoint_v1.BuildEndpointPath("clients");
        var request = new HttpRequestMessage(HttpMethod.Get, requestUri)
            .PopulateHeadersWith(new Dictionary<string, string?>
            {
                { "pageIndex", pageIndex.ToString() },
                { "pageSize", pageSize.ToString() },
                { "sortField", sortField },
                { "sortAsc", sortAsc.ToString() },
                { "filters", filters },
                { "projectionFields", projectionFields }
            });

        var responseMessage = await http.SendAsync(request, cancellationToken);
        var response = await responseMessage.Content.ReadFromJsonAsync<GetClientsResponse>(cancellationToken: cancellationToken);

        response.LogResponseDetails(http.BaseAddress + requestUri, HttpMethod.Get.Method, requestStartTime);
        response.HandledErrorResponse(toastService);

        return response;
    }

    /// <inheritdoc/>
    public async Task<GetClientByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var requestStartTime = DateTime.UtcNow;
        var requestUri = ApiEndpoint_v1.BuildEndpointPath($"client/{id}");
        var response = await http.GetFromJsonAsync<GetClientByIdResponse>(requestUri, cancellationToken: cancellationToken);

        response.LogResponseDetails(http.BaseAddress + requestUri, HttpMethod.Get.Method, requestStartTime);
        response.HandledErrorResponse(toastService);

        return response;
    }

    /// <inheritdoc/>
    public async Task<CreateClientResponse?> CreateAsync(ClientDto model, CancellationToken cancellationToken = default)
    {
        var requestStartTime = DateTime.UtcNow;
        var requestUri = ApiEndpoint_v1.BuildEndpointPath("client");
        var responseMessage = await http.PostAsJsonAsync(requestUri, model, cancellationToken: cancellationToken);
        var response = await responseMessage.Content.ReadFromJsonAsync<CreateClientResponse>(cancellationToken: cancellationToken);

        response.LogResponseDetails(http.BaseAddress + requestUri, HttpMethod.Post.Method, requestStartTime);
        response.HandledErrorResponse(toastService);

        return response;
    }

    /// <inheritdoc/>
    public async Task<UpdateClientResponse?> UpdateAsync(Guid id, ClientDto model, CancellationToken cancellationToken = default)
    {
        var requestStartTime = DateTime.UtcNow;
        var requestUri = ApiEndpoint_v1.BuildEndpointPath($"client/{id}");
        var responseMessage = await http.PutAsJsonAsync(requestUri, model, cancellationToken: cancellationToken);
        var response = await responseMessage.Content.ReadFromJsonAsync<UpdateClientResponse>(cancellationToken: cancellationToken);

        response.LogResponseDetails(http.BaseAddress + requestUri, HttpMethod.Put.Method, requestStartTime);
        response.HandledErrorResponse(toastService);

        return response;
    }

    /// <inheritdoc/>
    public async Task<DeleteClientResponse?> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var requestStartTime = DateTime.UtcNow;
        var requestUri = ApiEndpoint_v1.BuildEndpointPath($"client/{id}");
        var response = await http.DeleteFromJsonAsync<DeleteClientResponse>(requestUri, cancellationToken: cancellationToken);

        response.LogResponseDetails(http.BaseAddress + requestUri, HttpMethod.Delete.Method, requestStartTime);
        response.HandledErrorResponse(toastService);

        return response;
    }
}
