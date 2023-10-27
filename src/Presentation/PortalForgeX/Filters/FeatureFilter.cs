using Microsoft.FeatureManagement;
using PortalForgeX.Shared.Resources.Common;

namespace PortalForgeX.Filters;

public class FeatureFilter(IFeatureManager featureManager) : IEndpointFilter
{
    private const string FEATURE_FILTER_KEY = "endpoint.feature";

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        try
        {
            if (!context.HttpContext.Items.ContainsKey(FEATURE_FILTER_KEY))
            {
                return Results.Problem(FeaturesResources.FeatureDisabled);
            }

            var feature = context.HttpContext.Items[FEATURE_FILTER_KEY]?.ToString();
            if (feature is null || string.IsNullOrWhiteSpace(feature) || !await featureManager.IsEnabledAsync(feature))
            {
                return Results.Problem(FeaturesResources.FeatureDisabled);
            }

            return await next(context);
        }
        finally
        {
            context.HttpContext.Items.Remove(FEATURE_FILTER_KEY);
        }
    }

    public static EndpointFilterInvocationContext SetRequestingFeature(EndpointFilterInvocationContext context, string feature)
    {
        context.HttpContext.Items[FEATURE_FILTER_KEY] = feature;

        return context;
    }
}
