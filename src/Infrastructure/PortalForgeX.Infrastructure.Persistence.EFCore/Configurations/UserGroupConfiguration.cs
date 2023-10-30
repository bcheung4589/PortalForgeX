using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalForgeX.Domain.Entities;

namespace PortalForgeX.Persistence.EFCore.Configurations;

internal class UserGroupConfiguration : IEntityTypeConfiguration<UserGroup>
{
    public void Configure(EntityTypeBuilder<UserGroup> builder)
    {
        builder.HasMany(x => x.Profiles)
            .WithMany(x => x.Groups)
            .UsingEntity<UserInGroup>(
            x =>
            {
                x.HasOne(x => x.UserGroup).WithMany().HasForeignKey(x => x.UserGroupId);
                x.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
            });
    }
}
