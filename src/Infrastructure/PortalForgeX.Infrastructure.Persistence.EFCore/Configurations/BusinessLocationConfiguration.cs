using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalForgeX.Domain.Entities;
using PortalForgeX.Persistence.EFCore.ValueGenerators;

namespace PortalForgeX.Persistence.EFCore.Configurations;

internal class BusinessLocationConfiguration : IEntityTypeConfiguration<BusinessLocation>
{
    public void Configure(EntityTypeBuilder<BusinessLocation> builder)
    {
        builder.HasIndex(x => x.ApiKey).IsUnique();

        builder.Property(x => x.ApiKey)
                .HasValueGenerator<BusinessLocationApiKeyValueGenerator>()
                .ValueGeneratedOnAdd();
    }
}
