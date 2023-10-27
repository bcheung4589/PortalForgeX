using Blazored.Toast.Services;
using Newtonsoft.Json;
using PortalForgeX.Client.Extensions;
using PortalForgeX.Shared;
using PortalForgeX.Shared.Facades;
using PortalForgeX.Shared.Features.BusinessLocations;
using System.Net.Http.Json;

namespace PortalForgeX.Client.Facades;

/// <summary>
/// The HttpBusinessLocationFacade exposes all the API features 
/// available concerning business location entities.
/// </summary>
/// <param name="httpClientFactory"></param>
public class HttpBusinessLocationFacade(IHttpClientFactory httpClientFactory, IToastService toastService) : IBusinessLocationFacade
{
    private readonly HttpClient http = httpClientFactory.CreateServerClient();

    /// <inheritdoc/>
    public async Task<GetBusinessLocationsResponse?> GetAsync(int? pageIndex = null, int? pageSize = null, string? sortField = null, bool? sortAsc = null, string? filters = null, string? projectionFields = null, CancellationToken cancellationToken = default)
    {
        var requestStartTime = DateTime.UtcNow;
        var requestUri = ApiEndpoint_v1.BuildEndpointPath("businesslocations");
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
        var response = await responseMessage.Content.ReadFromJsonAsync<GetBusinessLocationsResponse>(cancellationToken: cancellationToken);

        response.LogResponseDetails(http.BaseAddress + requestUri, HttpMethod.Get.Method, requestStartTime);
        response.HandledErrorResponse(toastService);

        return response;
    }

    /// <inheritdoc/>
    public async Task<GetBusinessLocationByIdResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var requestStartTime = DateTime.UtcNow;
        var requestUri = ApiEndpoint_v1.BuildEndpointPath($"businesslocation/{id}");
        var response = await http.GetFromJsonAsync<GetBusinessLocationByIdResponse>(requestUri, cancellationToken: cancellationToken);

        response.LogResponseDetails(http.BaseAddress + requestUri, HttpMethod.Get.Method, requestStartTime);
        response.HandledErrorResponse(toastService);

        return response;
    }

    /// <inheritdoc/>
    public async Task<CreateBusinessLocationResponse?> CreateAsync(BusinessLocationDto model, CancellationToken cancellationToken = default)
    {
        var requestStartTime = DateTime.UtcNow;
        var requestUri = ApiEndpoint_v1.BuildEndpointPath("businesslocation");
        var responseMessage = await http.PostAsJsonAsync(requestUri, model, cancellationToken: cancellationToken);
        var response = await responseMessage.Content.ReadFromJsonAsync<CreateBusinessLocationResponse>(cancellationToken: cancellationToken);

        response.LogResponseDetails(http.BaseAddress + requestUri, HttpMethod.Post.Method, requestStartTime);
        response.HandledErrorResponse(toastService);

        return response;
    }

    /// <inheritdoc/>
    public async Task<UpdateBusinessLocationResponse?> UpdateAsync(int id, BusinessLocationDto model, CancellationToken cancellationToken = default)
    {
        var requestStartTime = DateTime.UtcNow;
        var requestUri = ApiEndpoint_v1.BuildEndpointPath($"businesslocation/{id}");
        var responseMessage = await http.PutAsJsonAsync(requestUri, model, cancellationToken: cancellationToken);
        var response = await responseMessage.Content.ReadFromJsonAsync<UpdateBusinessLocationResponse>(cancellationToken: cancellationToken);

        response.LogResponseDetails(http.BaseAddress + requestUri, HttpMethod.Put.Method, requestStartTime);
        response.HandledErrorResponse(toastService);

        return response;
    }

    /// <inheritdoc/>
    public async Task<DeleteBusinessLocationResponse?> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var requestStartTime = DateTime.UtcNow;
        var requestUri = ApiEndpoint_v1.BuildEndpointPath($"businesslocation/{id}");
        var httpResponse = await http.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri), cancellationToken: cancellationToken);
        var responseString = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

        DeleteBusinessLocationResponse? response;
        if (httpResponse.IsSuccessStatusCode)
        {
            response = JsonConvert.DeserializeObject<DeleteBusinessLocationResponse>(responseString);
        }
        else
        {
            response = new DeleteBusinessLocationResponse
            {
                HasError = true,
                ErrorMessages = new string[] { responseString.TrimStart('[').TrimEnd(']').Trim('"') }
            };
        }

        response.LogResponseDetails(http.BaseAddress + requestUri, HttpMethod.Delete.Method, requestStartTime);
        response.HandledErrorResponse(toastService);

        return response;
    }
}
