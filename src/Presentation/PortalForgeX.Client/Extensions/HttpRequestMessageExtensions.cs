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
}
