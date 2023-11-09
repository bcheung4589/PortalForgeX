using Blazored.Toast.Services;
using PortalForgeX.Client.Extensions;
using PortalForgeX.Shared;
using PortalForgeX.Shared.Facades;
using PortalForgeX.Shared.Features.Checkouts;
using PortalForgeX.Shared.Features.Clients;
using System.Net.Http.Json;

namespace PortalForgeX.Client.Facades;

/// <summary>
/// The HttpCheckoutFacade exposes all the API features 
/// available concerning checkouts entities.
/// </summary>
/// <param name="httpClientFactory"></param>
/// <param name="toastService"></param>
public class HttpCheckoutFacade(IHttpClientFactory httpClientFactory, IToastService toastService) : ICheckoutFacade
{
    private readonly HttpClient http = httpClientFactory.CreateServerClient();

    /// <inheritdoc/>
    public async Task<GetCheckoutByIdResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoint_v1.BuildEndpointPath($"checkout/{id}"));

        return await request.Execute<GetCheckoutByIdResponse>(http, toastService: toastService, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<CreateCheckoutResponse?> CreateAsync(CheckoutDto model, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, ApiEndpoint_v1.BuildEndpointPath("checkout"))
        {
            Content = JsonContent.Create(model)
        };

        return await request.Execute<CreateCheckoutResponse>(http, toastService: toastService, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<UpdateCheckoutResponse?> UpdateAsync(int id, CheckoutDto model, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, ApiEndpoint_v1.BuildEndpointPath($"checkout/{id}"))
        {
            Content = JsonContent.Create(model)
        };

        return await request.Execute<UpdateCheckoutResponse>(http, toastService: toastService, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<DeleteCheckoutResponse?> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, ApiEndpoint_v1.BuildEndpointPath($"checkout/{id}"));

        return await request.Execute<DeleteCheckoutResponse>(http, toastService: toastService, cancellationToken: cancellationToken);
    }
}
