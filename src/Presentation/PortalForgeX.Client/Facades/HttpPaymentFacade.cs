using Blazored.Toast.Services;
using PortalForgeX.Client.Extensions;
using PortalForgeX.Shared;
using PortalForgeX.Shared.Facades;
using PortalForgeX.Shared.Features.Payments;
using System.Net.Http.Json;

namespace PortalForgeX.Client.Facades;

/// <summary>
/// The HttpPaymentFacade exposes all the API features 
/// available concerning client entities.
/// </summary>
/// <param name="httpClientFactory"></param>
public class HttpPaymentFacade(IHttpClientFactory httpClientFactory, IToastService toastService) : IPaymentFacade
{
    private readonly HttpClient http = httpClientFactory.CreateServerClient();

    /// <inheritdoc/>
    public async Task<GetPaymentsResponse?> GetAsync(
        int? pageIndex = null,
        int? pageSize = null,
        string? sortField = null,
        bool? sortAsc = null,
        string? filters = null,
        string? projectionFields = null,
        CancellationToken cancellationToken = default)
    {
        var requestStartTime = DateTime.UtcNow;
        var requestUri = ApiEndpoint_v1.BuildEndpointPath("payments");
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
        var response = await responseMessage.Content.ReadFromJsonAsync<GetPaymentsResponse>(cancellationToken: cancellationToken);

        response.LogResponseDetails(http.BaseAddress + requestUri, HttpMethod.Get.Method, requestStartTime);
        response.HandledErrorResponse();

        return response;
    }

    /// <inheritdoc/>
    public async Task<GetPaymentByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var requestStartTime = DateTime.UtcNow;
        var requestUri = ApiEndpoint_v1.BuildEndpointPath($"payment/{id}");
        var response = await http.GetFromJsonAsync<GetPaymentByIdResponse>(requestUri, cancellationToken: cancellationToken);

        response.LogResponseDetails(http.BaseAddress + requestUri, HttpMethod.Get.Method, requestStartTime);
        response.HandledErrorResponse(toastService);

        return response;
    }

    /// <inheritdoc/>
    public async Task<CreatePaymentResponse?> CreateAsync(PaymentDto model, CancellationToken cancellationToken = default)
    {
        var requestStartTime = DateTime.UtcNow;
        var requestUri = ApiEndpoint_v1.BuildEndpointPath("payment");
        var responseMessage = await http.PostAsJsonAsync(requestUri, model, cancellationToken: cancellationToken);
        var response = await responseMessage.Content.ReadFromJsonAsync<CreatePaymentResponse>(cancellationToken: cancellationToken);

        response.LogResponseDetails(http.BaseAddress + requestUri, HttpMethod.Post.Method, requestStartTime);
        response.HandledErrorResponse(toastService);

        return response;
    }

    /// <inheritdoc/>
    public async Task<UpdatePaymentResponse?> UpdateAsync(Guid id, PaymentDto model, CancellationToken cancellationToken = default)
    {
        var requestStartTime = DateTime.UtcNow;
        var requestUri = ApiEndpoint_v1.BuildEndpointPath($"payment/{id}");
        var responseMessage = await http.PutAsJsonAsync(requestUri, model, cancellationToken: cancellationToken);
        var response = await responseMessage.Content.ReadFromJsonAsync<UpdatePaymentResponse>(cancellationToken: cancellationToken);

        response.LogResponseDetails(http.BaseAddress + requestUri, HttpMethod.Put.Method, requestStartTime);
        response.HandledErrorResponse(toastService);

        return response;
    }

    /// <inheritdoc/>
    public async Task<DeletePaymentResponse?> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var requestStartTime = DateTime.UtcNow;
        var requestUri = ApiEndpoint_v1.BuildEndpointPath($"payment/{id}");
        var response = await http.DeleteFromJsonAsync<DeletePaymentResponse>(requestUri, cancellationToken: cancellationToken);

        response.LogResponseDetails(http.BaseAddress + requestUri, HttpMethod.Delete.Method, requestStartTime);
        response.HandledErrorResponse(toastService);

        return response;
    }
}
