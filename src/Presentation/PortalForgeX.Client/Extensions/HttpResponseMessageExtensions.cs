using Blazored.Toast.Services;
using PortalForgeX.Shared;
using System.Net.Http.Json;

namespace PortalForgeX.Client.Extensions;

public static class HttpResponseMessageExtensions
{
    /// <summary>
    /// Log debug details into the console.
    /// </summary>
    /// <param name="requestUri"></param>
    /// <param name="requestMethod"></param>
    /// <param name="requestStartTime"></param>
    /// <param name="response"></param>
    public static void LogResponseDetails(this HttpResponseMessage source, Result? response, string requestUri, string requestMethod, DateTime requestStartTime)
    {
#if DEBUG
        Console.WriteLine("--- DEBUG DETAILS ---");
        Console.WriteLine($"| Request: ({requestMethod}) {requestUri}");
        Console.WriteLine($"| Request Starttime: {requestStartTime}");
        Console.WriteLine($"| Server Execution Duration: {response?.ServerExecutionDuration}");
        Console.WriteLine($"| Request Execution Duration: {DateTime.UtcNow - requestStartTime}");
        if (response is null)
        {
            Console.WriteLine($"| Response: NULL/Error");
        }
        Console.WriteLine("---------------------");
#endif
    }

    /// <summary>
    /// Handle any errors captured in the response.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="toastService"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task HandleErrorMessagesAsync(this HttpResponseMessage source, IToastService? toastService = null, CancellationToken cancellationToken = default)
    {
        var errorsResponse = await source.Content.ReadFromJsonAsync<IEnumerable<string>>(cancellationToken);
        if (errorsResponse is not null && errorsResponse.Any())
        {
            foreach (var errorMessage in errorsResponse)
            {
                toastService?.ShowError(errorMessage);
                Console.WriteLine(errorMessage);
            }

            return;
        }

        Console.WriteLine("Exception Response from Server.");
        toastService?.ShowError("Technical Server Error.");
        return;
    }
}