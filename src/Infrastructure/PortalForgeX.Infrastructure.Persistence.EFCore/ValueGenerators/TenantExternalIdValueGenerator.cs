using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using PortalForgeX.Persistence.EFCore;

namespace PortalForgeX.Persistence.EFCore.ValueGenerators;

internal class TenantExternalIdValueGenerator : ValueGenerator
{
    public override bool GeneratesTemporaryValues => false;

    protected override object? NextValue(EntityEntry entry)
    {
        var context = (PortalContext)entry.Context;

        return GenerateUniqueTransactionId(context);
    }

    private static string GenerateUniqueTransactionId(PortalContext context)
    {
        var random = new Random();
        string newId;

        do
        {
            // Use Guid.NewGuid() to generate the External ID
            newId = Guid.NewGuid().ToString("N");
        }
        while (context.Tenants.Any(x => x.ExternalId == newId));

        return newId;
    }
}
