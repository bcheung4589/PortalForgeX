using PortalForgeX.Shared.Features.UserGroups;

namespace PortalForgeX.Shared.Facades;

/// <summary>
/// The IUserGroupFacade exposes all the features concerning usergroup entities.
/// </summary>
public interface IUserGroupFacade : IAppFeatureFacade
{
    /// <summary>
    /// Get the usergroups.
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortField"></param>
    /// <param name="sortAsc"></param>
    /// <param name="filters"></param>
    /// <param name="projectionFields"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetUserGroupsResponse?> GetAsync(
        int? pageIndex = null,
        int? pageSize = null,
        string? sortField = null,
        bool? sortAsc = null,
        string? filters = null,
        string? projectionFields = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the usergroups for specified user.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<GetUserGroupsByUserIdResponse?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get usergroup by id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetUserGroupByIdResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create usergroup.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CreateUserGroupResponse?> CreateAsync(UserGroupDto model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update usergroup.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<UpdateUserGroupResponse?> UpdateAsync(int id, UserGroupDto model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete usergroup.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DeleteUserGroupResponse?> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add the specified user to all the usergroups in <paramref name="userGroupIds"/>.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="userGroupIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AddUserToGroupsResponse> AddUserToGroupsAsync(string userId, IEnumerable<int> userGroupIds, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove the specified user from all the usergroups in <paramref name="userGroupIds"/>.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="userGroupIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<RemoveUserFromGroupsResponse> RemoveUserToGroupsAsync(string userId, IEnumerable<int> userGroupIds, CancellationToken cancellationToken = default);
}
