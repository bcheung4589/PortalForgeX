using Microsoft.EntityFrameworkCore;
using PortalForgeX.Application.Repositories;
using PortalForgeX.Domain.Entities;
using PortalForgeX.Persistence.EFCore.Repositories.Internal;

namespace PortalForgeX.Persistence.EFCore.Repositories;

public class UserGroupRepository : LoadedRepositoryBase<UserGroup, int>, IUserGroupRepository
{
    public UserGroupRepository(DomainContext context) : base(context) { }

    protected override IQueryable<UserGroup> IncludedTable()
        => Table // # Default Includes
            .Include(x => x.Profiles);
}
