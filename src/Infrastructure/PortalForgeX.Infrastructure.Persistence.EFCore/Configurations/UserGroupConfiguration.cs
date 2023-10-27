using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalForgeX.Domain.Entities;

namespace PortalForgeX.Persistence.EFCore.Configurations;

internal class UserGroupConfiguration : IEntityTypeConfiguration<UserGroup>
{
    public void Configure(EntityTypeBuilder<UserGroup> builder)
    {
        builder.HasMany(x => x.Profiles)
            .WithMany()
            .UsingEntity<UserInGroup>();
    }
}
