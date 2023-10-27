using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using PortalForgeX.Persistence.EFCore;

namespace PortalForgeX.Persistence.EFCore.ValueGenerators;

internal class PaymentTransactionIdValueGenerator : ValueGenerator
{
    public override bool GeneratesTemporaryValues => false;

    protected override object? NextValue(EntityEntry entry)
    {
        var context = (DomainContext)entry.Context;

        return GenerateUniqueTransactionId(context);
    }

    private static string GenerateUniqueTransactionId(DomainContext context)
    {
        var random = new Random();
        string newId;

        do
        {
            // Generate a random portion of the ID
            var randomPart = random.Next(1000, 9999).ToString();

            // Use Guid.NewGuid() to generate the rest of the ID
            var guidPart = Guid.NewGuid().ToString("N")[..6];

            newId = randomPart + guidPart;
        }
        while (context.Payments.Any(x => x.TransactionId == newId));

        return newId;
    }
}
