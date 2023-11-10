using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.UserGroups;

namespace PortalForgeX.Application.Features.UserGroups;

public record RemoveUserFromGroupsRequest(string UserId, IEnumerable<int> UserGroupIds) : ICommand<RemoveUserFromGroupsResponse>
{
    public RemoveUserFromGroupsResponse NewResponse() => new();
}

internal sealed class RemoveUserFromGroupsHandler(
    ILogger<RemoveUserFromGroupsHandler> logger,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RemoveUserFromGroupsRequest, RemoveUserFromGroupsResponse>
{
    private readonly ILogger<RemoveUserFromGroupsHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<RemoveUserFromGroupsResponse> Handle(RemoveUserFromGroupsRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();
        if (!request.UserGroupIds.Any())
        {
            response.SetSuccess();
            return response;
        }

        try
        {
            var result = await _unitOfWork.Context.UserInGroups
                .Where(x => x.UserId == request.UserId && request.UserGroupIds.Contains(x.UserGroupId))
                .ExecuteDeleteAsync(cancellationToken: cancellationToken);

            if (result == 0)
            {
                response.SetFailure($"Failed removing user from groups.");
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