using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Clients;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.DTOs;
using PortalForgeX.Shared.Features.Clients;
using PortalForgeX.Shared.Features.UserGroups;

namespace PortalForgeX.Application.Features.UserGroups;

public record GetUserGroupsRequest(EntityPageSetting Settings) : ICommand<GetUserGroupsResponse>
{
    public GetUserGroupsResponse NewResponse() => new();
}

internal sealed class GetUserGroupsHandler(ILogger<GetUserGroupsHandler> logger, IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetUserGroupsRequest, GetUserGroupsResponse>
{
    private readonly ILogger<GetUserGroupsHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<GetUserGroupsResponse> Handle(GetUserGroupsRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        try
        {
            var result = await _unitOfWork.UserGroupRepository.GetPageAsync(request.Settings, cancellationToken);

            response.SetSuccess(_mapper.Map<PagedList<UserGroupDto>>(result));
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, ex.Message);
            response.SetFailure("There was a technical error.", StatusCodes.Status500InternalServerError);
        }

        return response;
    }
}
