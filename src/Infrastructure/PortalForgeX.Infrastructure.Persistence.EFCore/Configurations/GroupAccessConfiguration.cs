using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalForgeX.Domain.Entities;

namespace PortalForgeX.Persistence.EFCore.Configurations;

internal class GroupAccessConfiguration : IEntityTypeConfiguration<GroupAccess>
{
    public void Configure(EntityTypeBuilder<GroupAccess> builder)
    {
        builder.HasKey(x => new { x.UserGroupId, x.Name });

        builder.HasOne(x => x.UserGroup)
            .WithMany();
    }
}