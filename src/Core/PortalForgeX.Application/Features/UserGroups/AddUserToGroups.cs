using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Domain.Entities;
using PortalForgeX.Shared.Features.UserGroups;

namespace PortalForgeX.Application.Features.UserGroups;

public record AddUserToGroupsRequest(string UserId, IEnumerable<int> UserGroupIds) : ICommand<AddUserToGroupsResponse>
{
    public AddUserToGroupsResponse NewResponse() => new();
}

internal sealed class AddUserToGroupsHandler(
    ILogger<AddUserToGroupsHandler> logger,
    IUnitOfWork unitOfWork)
    : IRequestHandler<AddUserToGroupsRequest, AddUserToGroupsResponse>
{
    private readonly ILogger<AddUserToGroupsHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<AddUserToGroupsResponse> Handle(AddUserToGroupsRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();
        if (!request.UserGroupIds.Any())
        {
            response.SetSuccess();
            return response;
        }

        try
        {
            var addingEntities = new List<UserInGroup>();
            foreach (var userGroupId in request.UserGroupIds)
            {
                addingEntities.Add(new()
                {
                    UserId = request.UserId,
                    UserGroupId = userGroupId
                });
            }

            await _unitOfWork.Context.UserInGroups.AddRangeAsync(addingEntities, cancellationToken);
            var result = await _unitOfWork.Context.SaveChangesAsync(cancellationToken);
            if (result == 0)
            {
                response.SetFailure($"Failed adding user to groups.");
                return response;
            }

            response.SetSuccess();
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, ex.Message);
            response.SetFailure("There was a technical error.", StatusCodes.Status500InternalServerError);
        }

        return response;
    }
}