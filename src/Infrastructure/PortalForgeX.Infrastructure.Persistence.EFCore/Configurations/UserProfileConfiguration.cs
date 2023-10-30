using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalForgeX.Domain.Entities;
using PortalForgeX.Domain.Entities.Tenants;

namespace PortalForgeX.Persistence.EFCore.Configurations;

internal class UserProfileConfiguration : IEntityTypeConfiguration<TenantUserProfile>
{
    public void Configure(EntityTypeBuilder<TenantUserProfile> builder)
    {
        builder.HasKey(x => x.UserId);

        builder.HasMany(x => x.Groups)
            .WithMany(x => x.Profiles)
            .UsingEntity<UserInGroup>(
            x =>
            {
                x.HasOne(x => x.UserGroup).WithMany().HasForeignKey(x => x.UserGroupId);
                x.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
            });
    }
}
