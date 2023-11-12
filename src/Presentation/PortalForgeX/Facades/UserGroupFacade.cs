using MediatR;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.UserGroups;
using PortalForgeX.Shared.Facades;
using PortalForgeX.Shared.Features.UserGroups;

namespace PortalForgeX.Facades;

/// <inheritdoc/>
public class UserGroupFacade(ISender sender) : IUserGroupFacade
{
    private const int DEFAULT_PAGE_SIZE = 25;

    /// <inheritdoc/>
    public async Task<GetUserGroupsResponse?> GetAsync(
        int? pageIndex = null,
        int? pageSize = null,
        string? sortField = null,
        bool? sortAsc = null,
        string? filters = null,
        string? projectionFields = null,
        CancellationToken cancellationToken = default)
        => await sender.Send(new GetUserGroupsRequest(new EntityPageSetting(
                pageSize ?? DEFAULT_PAGE_SIZE,
                pageIndex,
                sortField,
                sortAsc,
                filters,
                projectionFields)
            ), cancellationToken);

    /// <inheritdoc/>
    public async Task<GetUserGroupsByUserIdResponse?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        => await sender.Send(new GetUserGroupsByUserIdRequest(userId), cancellationToken);

    /// <inheritdoc/>
    public async Task<GetUserGroupByIdResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await sender.Send(new GetUserGroupByIdRequest(id), cancellationToken);

    /// <inheritdoc/>
    public async Task<CreateUserGroupResponse?> CreateAsync(UserGroupDto model, CancellationToken cancellationToken = default)
        => await sender.Send(new CreateUserGroupRequest(model), cancellationToken);

    /// <inheritdoc/>
    public async Task<UpdateUserGroupResponse?> UpdateAsync(int id, UserGroupDto model, CancellationToken cancellationToken = default)
        => await sender.Send(new UpdateUserGroupRequest(id, model), cancellationToken);

    /// <inheritdoc/>
    public async Task<DeleteUserGroupResponse?> DeleteAsync(int id, CancellationToken cancellationToken = default)
        => await sender.Send(new DeleteUserGroupRequest(id), cancellationToken);

    /// <inheritdoc/>
    public async Task<AddUserToGroupsResponse> AddUserToGroupsAsync(string userId, IEnumerable<int> userGroupIds, CancellationToken cancellationToken = default)
        => await sender.Send(new AddUserToGroupsRequest(userId, userGroupIds), cancellationToken);

    /// <inheritdoc/>
    public async Task<RemoveUserFromGroupsResponse> RemoveUserFromGroupsAsync(string userId, IEnumerable<int> userGroupIds, CancellationToken cancellationToken = default)
        => await sender.Send(new RemoveUserFromGroupsRequest(userId, userGroupIds), cancellationToken);
}
