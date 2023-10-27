using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System.Reflection;

namespace PortalForgeX.Infrastructure.Middleware;

/// <summary>
/// Capture any uncaught Exception and collect the request, response and user data.
/// Log the data and respond appropriately.
/// </summary>
public class ExceptionLoggingMiddleware : IMiddleware
{
    private static readonly ILogger Logger = Log.ForContext(MethodBase.GetCurrentMethod()?.DeclaringType!);

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ApplicationException ex)
        {
            BindResponseBody(context, out var originalResponseBody, out var placeholderStream);
            await SetResponseToServerError(context, ex.Message);

            var data = await LoadContextData(context, originalResponseBody, placeholderStream);
            data.Add("_Exception.Data", ex.Data);

            var dataAsString = JsonConvert.SerializeObject(data);
            Logger.Fatal(ex, "{Message}\n\t Data: {dataAsString}", ex.Message, dataAsString);
        }
        catch (Exception ex)
        {
            BindResponseBody(context, out var originalResponseBody, out var placeholderStream);
            await SetResponseToServerError(context, ex.Message);

            var dataAsString = JsonConvert.SerializeObject(await LoadContextData(context, originalResponseBody, placeholderStream));
            Logger.Fatal(ex, "{Message}\n\t Data: {dataAsString}", ex.Message, dataAsString);
        }
    }

    /// <summary>
    /// Binding the Response to a MemoryStream gives us the ability to read the Response.Body and reset its position.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="originalResponseBody"></param>
    /// <param name="placeholderStream"></param>
    private static void BindResponseBody(HttpContext context, out Stream originalResponseBody, out MemoryStream placeholderStream)
    {
        originalResponseBody = context.Response.Body;
        placeholderStream = new MemoryStream();
        context.Response.Body = placeholderStream;
    }

    /// <summary>
    /// Set the Response to StatusCodes.Status500InternalServerError and write the provided message to the Response.Body.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    private static async Task SetResponseToServerError(HttpContext context, string message)
    {
        var response = context.Response;
        response.StatusCode = StatusCodes.Status500InternalServerError;
        await response.WriteAsync(message);
    }

    /// <summary>
    /// Load the Request, Response and User Contexts data into a Dictionary.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="responseBody"></param>
    /// <param name="placeholderStream"></param>
    /// <returns></returns>
    private static async Task<IDictionary<object, object>> LoadContextData(HttpContext context, Stream responseBody, MemoryStream placeholderStream)
    {
        var data = new Dictionary<object, object>();

        // collect request data
        var request = context.Request;
        if (request is not null)
        {
            IFormCollection? form = null;
            var formString = string.Empty;
            var bodyString = await ReadRequestBody(context);
            if (request.HasFormContentType)
            {
                form = request.Form;
            }
            else
            {
                formString = bodyString;
            }

            data.Add("_RequestContext", JsonConvert.SerializeObject(new
            {
                Url = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString.Value}",
                Body = bodyString,
                RemoteIpAddress = context.Connection?.RemoteIpAddress?.ToString(),
                Form = form is not null ? JsonConvert.SerializeObject(form) : formString,
                request.Headers,
                request.Protocol,
                request.Method,
                request.IsHttps,
            }));
        }

        // collect response data
        var response = context.Response;
        if (response is not null)
        {
            data.Add("_ResponseContext", JsonConvert.SerializeObject(new
            {
                Body = await ReadResponseBody(context, responseBody, placeholderStream),
                response.StatusCode,
                response.Headers,
                response.Cookies
            }));
        }

        // collect user data
        data.Add("_UserContext", JsonConvert.SerializeObject(context.User.Identity));

        return data;
    }

    /// <summary>
    /// Read the Response Body and return it as string.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="responseBody"></param>
    /// <param name="placeholderStream"></param>
    /// <returns></returns>
    private static async Task<string> ReadResponseBody(HttpContext context, Stream responseBody, MemoryStream placeholderStream)
    {
        placeholderStream.Seek(0, SeekOrigin.Begin);

        using var reader = new StreamReader(context.Response.Body);
        var responseBodyString = await reader.ReadToEndAsync();

        placeholderStream.Seek(0, SeekOrigin.Begin);
        await placeholderStream.CopyToAsync(responseBody);

        return responseBodyString;
    }

    /// <summary>
    /// Read the Request Body and return it as string.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private static async Task<string> ReadRequestBody(HttpContext context)
    {
        context.Request.Body.Position = 0;

        using var reader = new StreamReader(context.Request.Body);
        var requestBody = await reader.ReadToEndAsync();

        context.Request.Body.Position = 0;

        return requestBody;
    }
}
