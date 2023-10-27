using Microsoft.FeatureManagement;
using PortalForgeX.Shared.Resources.Common;

namespace PortalForgeX.Infrastructure.FeatureManagement;

public static class FeatureDefinitionExtensions
{
    /// <summary>
    /// The DisplayName gets pulled from the Features Resources.
    /// </summary>
    public static string GetDisplayName(this FeatureDefinition source)
    {
        var resourceValue = FeaturesResources.ResourceManager.GetString($"{source.Name}_DisplayName");

        if (string.IsNullOrWhiteSpace(resourceValue))
        {
            return $"No resource ({source.Name}).";
        }

        return resourceValue;
    }
}
