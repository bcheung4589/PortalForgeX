using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalForgeX.Domain.Entities.Tenants;
using PortalForgeX.Persistence.EFCore.ValueGenerators;

namespace PortalForgeX.Persistence.EFCore.Configurations;

internal class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasIndex(x => x.ExternalId).IsUnique();

        builder.Property(x => x.ExternalId)
                .HasValueGenerator<TenantExternalIdValueGenerator>()
                .ValueGeneratedOnAdd();

        builder.HasOne(x => x.Manager)
            .WithMany()
            .HasForeignKey(x => x.ManagerId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
