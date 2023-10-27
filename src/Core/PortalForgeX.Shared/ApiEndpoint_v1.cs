using PortalForgeX.Shared.Extensions;

namespace PortalForgeX.Shared;

public abstract class ApiEndpoint_v1
{
    public const string PREFIX = $"api/{VERSION}/";
    public const string VERSION = "v1";

    public static string BuildEndpointPath(string path)
        => string.Concat(PREFIX, path).RemoveDoubleSlashes();
}