namespace PortalForgeX.Endpoints.Internal;

public interface IFeatureEndpoint
{
    //string FeatureName { get; }

    void AddRoutes(IEndpointRouteBuilder app);
}
