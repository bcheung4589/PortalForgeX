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
/// <param name="toastService"></param>
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
        var request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoint_v1.BuildEndpointPath("payments"))
            .PopulateHeadersWith(new Dictionary<string, string?>
            {
                { "pageIndex", pageIndex.ToString() },
                { "pageSize", pageSize.ToString() },
                { "sortField", sortField },
                { "sortAsc", sortAsc.ToString() },
                { "filters", filters },
                { "projectionFields", projectionFields }
            });

        return await request.Execute<GetPaymentsResponse>(http, toastService: toastService, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<GetPaymentByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoint_v1.BuildEndpointPath($"payment/{id}"));

        return await request.Execute<GetPaymentByIdResponse>(http, toastService: toastService, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<CreatePaymentResponse?> CreateAsync(PaymentDto model, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, ApiEndpoint_v1.BuildEndpointPath("payment"))
        {
            Content = JsonContent.Create(model)
        };

        return await request.Execute<CreatePaymentResponse>(http, toastService: toastService, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<UpdatePaymentResponse?> UpdateAsync(Guid id, PaymentDto model, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, ApiEndpoint_v1.BuildEndpointPath($"payment/{id}"))
        {
            Content = JsonContent.Create(model)
        };

        return await request.Execute<UpdatePaymentResponse>(http, toastService: toastService, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<DeletePaymentResponse?> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, ApiEndpoint_v1.BuildEndpointPath($"payment/{id}"));

        return await request.Execute<DeletePaymentResponse>(http, toastService: toastService, cancellationToken: cancellationToken);
    }
}
