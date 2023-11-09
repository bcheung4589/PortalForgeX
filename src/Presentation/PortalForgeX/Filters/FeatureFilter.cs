using Microsoft.FeatureManagement;
using PortalForgeX.Shared.Resources.Common;

namespace PortalForgeX.Filters;

public class FeatureFilter(IFeatureManager featureManager, string featureName) : IEndpointFilter
{
    private readonly string _featureName = featureName;

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (string.IsNullOrWhiteSpace(_featureName) || !await featureManager.IsEnabledAsync(_featureName))
        {
            return Results.Problem(FeaturesResources.FeatureDisabled);
        }

        return await next(context);
    }
}
