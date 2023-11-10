using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.UserGroups;

namespace PortalForgeX.Application.Features.UserGroups;

public record GetUserGroupsByUserIdRequest(string UserId) : ICommand<GetUserGroupsByUserIdResponse>
{
    public GetUserGroupsByUserIdResponse NewResponse() => new();
}

internal sealed class GetUserGroupsByUserIdHandler(ILogger<GetUserGroupsByUserIdHandler> logger, IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetUserGroupsByUserIdRequest, GetUserGroupsByUserIdResponse>
{
    private readonly ILogger<GetUserGroupsByUserIdHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<GetUserGroupsByUserIdResponse> Handle(GetUserGroupsByUserIdRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        try
        {
            var userGroupIds = _unitOfWork.Context.UserInGroups
                .Where(x => x.UserId == request.UserId)
                .Select(x => x.UserGroupId);

            if (!await userGroupIds.AnyAsync(cancellationToken: cancellationToken))
            {
                response.SetSuccess();
                return response;
            }

            var result = await _unitOfWork.UserGroupRepository
                .GetAsQuery()
                .Where(x => userGroupIds.Contains(x.Id))
                .ToListAsync(cancellationToken: cancellationToken);

            if (result is null)
            {
                response.SetFailure($"Failure getting usergroups for User ({request.UserId}).", StatusCodes.Status404NotFound);
                return response;
            }

            response.SetSuccess(_mapper.Map<List<UserGroupDto>>(result));
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, ex.Message);
            response.SetFailure("There was a technical error.", StatusCodes.Status500InternalServerError);
        }

        return response;
    }
}
