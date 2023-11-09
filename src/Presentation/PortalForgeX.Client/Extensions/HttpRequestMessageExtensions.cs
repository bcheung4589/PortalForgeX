using Blazored.Toast.Services;
using PortalForgeX.Shared;
using System.Net.Http.Json;

namespace PortalForgeX.Client.Extensions;

public static class HttpRequestMessageExtensions
{
    /// <summary>
    /// Add the Dictionairy as Headers to the Request.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="paramValues"></param>
    /// <returns></returns>
    public static HttpRequestMessage PopulateHeadersWith(this HttpRequestMessage request, IDictionary<string, string?> paramValues)
    {
        foreach (var param in paramValues)
        {
            if (string.IsNullOrWhiteSpace(param.Value))
            {
                continue;
            }

            request.Headers.Add(param.Key, param.Value);
        }

        return request;
    }

    /// <summary>
    /// Execute the Request with the given <paramref name="http"/> and handle any error messages with the <paramref name="toastService"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="request"></param>
    /// <param name="http"></param>
    /// <param name="toastService"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<T?> Execute<T>(this HttpRequestMessage request, HttpClient http, IToastService? toastService = null, CancellationToken cancellationToken = default) where T : Result
    {
        var requestStartTime = DateTime.UtcNow;
        var responseMessage = await http.SendAsync(request, cancellationToken);
        if (!responseMessage.IsSuccessStatusCode)
        {
            await responseMessage.HandleErrorMessagesAsync(toastService, cancellationToken: cancellationToken);
            responseMessage.LogResponseDetails(null, http.BaseAddress + request.RequestUri?.ToString(), request.Method.Method, requestStartTime);

            return null;
        }

        var response = await responseMessage.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
        responseMessage.LogResponseDetails(response, http.BaseAddress + request.RequestUri?.ToString(), request.Method.Method, requestStartTime);

        return response;
    }
}
