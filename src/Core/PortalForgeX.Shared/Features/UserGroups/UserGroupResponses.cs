using PortalForgeX.Shared.DTOs;

namespace PortalForgeX.Shared.Features.UserGroups;

public record GetUserGroupsResponse : Result<PagedList<UserGroupDto>>;

public record GetUserGroupsByUserIdResponse : Result<List<UserGroupDto>>;

public record GetUserGroupByIdResponse : Result<UserGroupDto>;

public record CreateUserGroupResponse : Result<UserGroupDto>;

public record UpdateUserGroupResponse : Result<UserGroupDto>;

public record DeleteUserGroupResponse : Result;

public record AddUserToGroupsResponse : Result;

public record RemoveUserFromGroupsResponse : Result;
