using Blazored.Toast.Services;
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
/// <param name="toastService"></param>
public class HttpBusinessLocationFacade(IHttpClientFactory httpClientFactory, IToastService toastService) : IBusinessLocationFacade
{
    private readonly HttpClient http = httpClientFactory.CreateServerClient();

    /// <inheritdoc/>
    public async Task<GetBusinessLocationsResponse?> GetAsync(
        int? pageIndex = null,
        int? pageSize = null,
        string? sortField = null,
        bool? sortAsc = null,
        string? filters = null,
        string? projectionFields = null,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoint_v1.BuildEndpointPath("businesslocations"))
            .PopulateHeadersWith(new Dictionary<string, string?>
            {
                { "pageIndex", pageIndex.ToString() },
                { "pageSize", pageSize.ToString() },
                { "sortField", sortField },
                { "sortAsc", sortAsc.ToString() },
                { "filters", filters },
                { "projectionFields", projectionFields }
            });

        return await request.Execute<GetBusinessLocationsResponse>(http, toastService: toastService, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<GetBusinessLocationByIdResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoint_v1.BuildEndpointPath($"businesslocation/{id}"));

        return await request.Execute<GetBusinessLocationByIdResponse>(http, toastService: toastService, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<CreateBusinessLocationResponse?> CreateAsync(BusinessLocationDto model, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, ApiEndpoint_v1.BuildEndpointPath("businesslocation"))
        {
            Content = JsonContent.Create(model)
        };

        return await request.Execute<CreateBusinessLocationResponse>(http, toastService: toastService, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<UpdateBusinessLocationResponse?> UpdateAsync(int id, BusinessLocationDto model, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, ApiEndpoint_v1.BuildEndpointPath($"businesslocation/{id}"))
        {
            Content = JsonContent.Create(model)
        };

        return await request.Execute<UpdateBusinessLocationResponse>(http, toastService: toastService, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<DeleteBusinessLocationResponse?> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, ApiEndpoint_v1.BuildEndpointPath($"businesslocation/{id}"));

        return await request.Execute<DeleteBusinessLocationResponse>(http, toastService: toastService, cancellationToken: cancellationToken);
    }
}
