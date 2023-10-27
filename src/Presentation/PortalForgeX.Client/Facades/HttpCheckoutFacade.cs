using Blazored.Toast.Services;
using PortalForgeX.Client.Extensions;
using PortalForgeX.Shared;
using PortalForgeX.Shared.Facades;
using PortalForgeX.Shared.Features.Checkouts;
using System.Net.Http.Json;

namespace PortalForgeX.Client.Facades;

/// <summary>
/// The HttpCheckoutFacade exposes all the API features 
/// available concerning checkouts entities.
/// </summary>
/// <param name="httpClientFactory"></param>
public class HttpCheckoutFacade(IHttpClientFactory httpClientFactory, IToastService toastService) : ICheckoutFacade
{
    private readonly HttpClient http = httpClientFactory.CreateServerClient();

    /// <inheritdoc/>
    public async Task<GetCheckoutByIdResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var requestStartTime = DateTime.UtcNow;
        var requestUri = ApiEndpoint_v1.BuildEndpointPath($"checkout/{id}");
        var response = await http.GetFromJsonAsync<GetCheckoutByIdResponse>(requestUri, cancellationToken: cancellationToken);

        response.LogResponseDetails(http.BaseAddress + requestUri, HttpMethod.Get.Method, requestStartTime);
        response.HandledErrorResponse(toastService);

        return response;
    }

    /// <inheritdoc/>
    public async Task<CreateCheckoutResponse?> CreateAsync(CheckoutDto model, CancellationToken cancellationToken = default)
    {
        var requestStartTime = DateTime.UtcNow;
        var requestUri = ApiEndpoint_v1.BuildEndpointPath("checkout");
        var responseMessage = await http.PostAsJsonAsync(requestUri, model, cancellationToken: cancellationToken);
        var response = await responseMessage.Content.ReadFromJsonAsync<CreateCheckoutResponse>(cancellationToken: cancellationToken);

        response.LogResponseDetails(http.BaseAddress + requestUri, HttpMethod.Post.Method, requestStartTime);
        response.HandledErrorResponse(toastService);

        return response;
    }

    /// <inheritdoc/>
    public async Task<UpdateCheckoutResponse?> UpdateAsync(int id, CheckoutDto model, CancellationToken cancellationToken = default)
    {
        var requestStartTime = DateTime.UtcNow;
        var requestUri = ApiEndpoint_v1.BuildEndpointPath($"checkout/{id}");
        var responseMessage = await http.PutAsJsonAsync(requestUri, model, cancellationToken: cancellationToken);
        var response = await responseMessage.Content.ReadFromJsonAsync<UpdateCheckoutResponse>(cancellationToken: cancellationToken);

        response.LogResponseDetails(http.BaseAddress + requestUri, HttpMethod.Put.Method, requestStartTime);
        response.HandledErrorResponse(toastService);

        return response;
    }

    /// <inheritdoc/>
    public async Task<DeleteCheckoutResponse?> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var requestStartTime = DateTime.UtcNow;
        var requestUri = ApiEndpoint_v1.BuildEndpointPath($"checkout/{id}");
        var response = await http.DeleteFromJsonAsync<DeleteCheckoutResponse>(requestUri, cancellationToken: cancellationToken);

        response.LogResponseDetails(http.BaseAddress + requestUri, HttpMethod.Delete.Method, requestStartTime);
        response.HandledErrorResponse(toastService);

        return response;
    }
}
