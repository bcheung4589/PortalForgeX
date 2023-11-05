using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace PortalForgeX.Persistence.EFCore.ValueGenerators;

internal class BusinessLocationApiKeyValueGenerator : ValueGenerator
{
    public override bool GeneratesTemporaryValues => false;

    protected override object? NextValue(EntityEntry entry)
    {
        var context = (DomainContext)entry.Context;

        return GenerateUniqueApiKey(context);
    }

    private static string GenerateUniqueApiKey(DomainContext context)
    {
        string newApiKey;

        do
        {
            // Use Guid.NewGuid() to generate the ApiKey
            newApiKey = Guid.NewGuid().ToString("N");
        }
        while (context.BusinessLocations.Any(x => x.ApiKey == newApiKey));

        return newApiKey;
    }
}
