using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.UserGroups;

namespace PortalForgeX.Application.Features.UserGroups;

public record DeleteUserGroupRequest(int Id) : ICommand<DeleteUserGroupResponse>
{
    public DeleteUserGroupResponse NewResponse() => new();
}

internal sealed class DeleteUserGroupHandler(ILogger<DeleteUserGroupHandler> logger, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteUserGroupRequest, DeleteUserGroupResponse>
{
    private readonly ILogger<DeleteUserGroupHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<DeleteUserGroupResponse> Handle(DeleteUserGroupRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        try
        {
            var foundObject = await _unitOfWork.UserGroupRepository.GetByIdAsync(request.Id, cancellationToken);
            if (foundObject is null)
            {
                response.SetFailure($"UserGroup with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
                return response;
            }

            _ = await _unitOfWork.UserGroupRepository.DeleteAsync(foundObject, cancellationToken);
            _ = await _unitOfWork.SaveChangesAsync(cancellationToken);

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
