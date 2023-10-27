using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalForgeX.Domain.Entities;
using PortalForgeX.Persistence.EFCore.ValueGenerators;

namespace PortalForgeX.Persistence.EFCore.Configurations;

internal class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasIndex(x => x.TransactionId).IsUnique();

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2);

        builder.Property(x => x.TransactionId)
                .HasValueGenerator<PaymentTransactionIdValueGenerator>()
                .ValueGeneratedOnAdd();
    }
}
