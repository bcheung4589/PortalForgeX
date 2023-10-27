namespace PortalForgeX.Client.Extensions;

public static class IHttpClientFactoryExtensions
{
    public const string SERVER_API_CLIENT_NAME = "PortalForgeX.ServerAPI";

    /// <summary>
    /// Return a named HttpClient that connects to the Server.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static HttpClient CreateServerClient(this IHttpClientFactory source)
        => source.CreateClient(SERVER_API_CLIENT_NAME);
}
