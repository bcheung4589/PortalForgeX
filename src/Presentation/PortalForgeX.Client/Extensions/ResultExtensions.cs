using Blazored.Toast.Services;
using PortalForgeX.Shared;

namespace PortalForgeX.Client.Extensions;

public static class ResultExtensions
{
    /// <summary>
    /// Log debug details into the console.
    /// </summary>
    /// <param name="requestUri"></param>
    /// <param name="requestMethod"></param>
    /// <param name="requestStartTime"></param>
    /// <param name="response"></param>
    public static void LogResponseDetails(this Result? response, string requestUri, string requestMethod, DateTime requestStartTime)
    {
#if DEBUG
        Console.WriteLine("--- DEBUG DETAILS ---");
        Console.WriteLine($"| Request: ({requestMethod}) {requestUri}");
        Console.WriteLine($"| Request Starttime: {requestStartTime}");
        Console.WriteLine($"| Server Execution Duration: {response?.ServerExecutionDuration}");
        Console.WriteLine($"| Request Execution Duration: {DateTime.UtcNow - requestStartTime}");
        if (response is null)
        {
            Console.WriteLine($"| Response: NULL");
        }
        Console.WriteLine("---------------------");
#endif
    }

    /// <summary>
    /// Handle the result and process the error messages if there are any.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="toastService"></param>
    /// <returns></returns>
    public static bool HandledErrorResponse(this Result? result, IToastService? toastService = null)
    {
        if (result is null)
        {
            Console.WriteLine("Exception Response from Server.");
            toastService?.ShowError("Technical Server Error.");
            return true;
        }

        if (result.HasError)
        {
            if (result.ErrorMessages is not null && result.ErrorMessages.Any())
            {
                foreach (var errorMessage in result.ErrorMessages)
                {
                    toastService?.ShowError(errorMessage);
                    Console.WriteLine(errorMessage);
                }
            }
            return true;
        }

        return false;
    }
}
