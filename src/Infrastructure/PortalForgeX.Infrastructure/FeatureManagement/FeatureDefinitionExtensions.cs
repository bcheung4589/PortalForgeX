using Microsoft.FeatureManagement;
using PortalForgeX.Shared.Resources.Common;

namespace PortalForgeX.Infrastructure.FeatureManagement;

public static class FeatureDefinitionExtensions
{
    /// <summary>
    /// The DisplayName gets pulled from the Features Resources.
    /// </summary>
    /// <param name="source"></param>
    /// <returns>DisplayName from FeaturesResources; if null, returns Feature Name.</returns>
    public static string GetDisplayName(this FeatureDefinition source)
    {
        var resourceValue = FeaturesResources.ResourceManager.GetString($"{source.Name}_DisplayName");

        return !string.IsNullOrWhiteSpace(resourceValue)
            ? resourceValue
            : source.Name;
    }
}
