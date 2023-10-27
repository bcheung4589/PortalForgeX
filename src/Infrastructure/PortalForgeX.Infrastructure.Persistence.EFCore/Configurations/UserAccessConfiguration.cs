using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalForgeX.Domain.Entities;

namespace PortalForgeX.Persistence.EFCore.Configurations;

internal class UserAccessConfiguration : IEntityTypeConfiguration<UserAccess>
{
    public void Configure(EntityTypeBuilder<UserAccess> builder)
    {
        builder.HasKey(x => new { x.UserProfileId, x.Name });

        builder.HasOne(x => x.UserProfile)
            .WithMany();
    }
}
