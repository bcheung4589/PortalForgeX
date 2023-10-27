using Blazored.Toast.Services;
using PortalForgeX.Client.Extensions;
using PortalForgeX.Shared;
using PortalForgeX.Shared.Facades;
using PortalForgeX.Shared.Features.ClientContacts;
using System.Net.Http.Json;

namespace PortalForgeX.Client.Facades;

/// <summary>
/// The HttpClientContactFacade exposes all the API features 
/// available concerning client contact entities.
/// </summary>
/// <param name="httpClientFactory"></param>
public class HttpClientContactFacade(IHttpClientFactory httpClientFactory, IToastService toastService) : IClientContactFacade
{
    private readonly HttpClient http = httpClientFactory.CreateServerClient();

    /// <inheritdoc/>
    public async Task<GetClientContactByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var requestStartTime = DateTime.UtcNow;
        var requestUri = ApiEndpoint_v1.BuildEndpointPath($"clientcontact/{id}");
        var response = await http.GetFromJsonAsync<GetClientContactByIdResponse>(requestUri, cancellationToken: cancellationToken);

        response.LogResponseDetails(http.BaseAddress + requestUri, HttpMethod.Get.Method, requestStartTime);
        response.HandledErrorResponse(toastService);

        return response;
    }

    /// <inheritdoc/>
    public async Task<CreateClientContactResponse?> CreateAsync(ClientContactDto model, CancellationToken cancellationToken = default)
    {
        var requestStartTime = DateTime.UtcNow;
        var requestUri = ApiEndpoint_v1.BuildEndpointPath("clientcontact");
        var responseMessage = await http.PostAsJsonAsync(requestUri, model, cancellationToken: cancellationToken);
        var response = await responseMessage.Content.ReadFromJsonAsync<CreateClientContactResponse>(cancellationToken: cancellationToken);

        response.LogResponseDetails(http.BaseAddress + requestUri, HttpMethod.Post.Method, requestStartTime);
        response.HandledErrorResponse(toastService);

        return response;
    }

    /// <inheritdoc/>
    public async Task<UpdateClientContactResponse?> UpdateAsync(Guid id, ClientContactDto model, CancellationToken cancellationToken = default)
    {
        var requestStartTime = DateTime.UtcNow;
        var requestUri = ApiEndpoint_v1.BuildEndpointPath($"clientcontact/{id}");
        var responseMessage = await http.PutAsJsonAsync(requestUri, model, cancellationToken: cancellationToken);
        var response = await responseMessage.Content.ReadFromJsonAsync<UpdateClientContactResponse>(cancellationToken: cancellationToken);

        response.LogResponseDetails(http.BaseAddress + requestUri, HttpMethod.Put.Method, requestStartTime);
        response.HandledErrorResponse(toastService);

        return response;
    }

    /// <inheritdoc/>
    public async Task<DeleteClientContactResponse?> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var requestStartTime = DateTime.UtcNow;
        var requestUri = ApiEndpoint_v1.BuildEndpointPath($"clientcontact/{id}");
        var response = await http.DeleteFromJsonAsync<DeleteClientContactResponse>(requestUri, cancellationToken: cancellationToken);

        response.LogResponseDetails(http.BaseAddress + requestUri, HttpMethod.Delete.Method, requestStartTime);
        response.HandledErrorResponse(toastService);

        return response;
    }
}
