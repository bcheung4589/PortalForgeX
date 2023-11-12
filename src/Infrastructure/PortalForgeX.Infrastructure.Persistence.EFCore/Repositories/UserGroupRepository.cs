using PortalForgeX.Application.Repositories;
using PortalForgeX.Domain.Entities;
using PortalForgeX.Persistence.EFCore.Repositories.Internal;

namespace PortalForgeX.Persistence.EFCore.Repositories;

public class UserGroupRepository(DomainContext context) : EfRepositoryBase<UserGroup, int>(context), IUserGroupRepository
{
}
