using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.UserGroups;

namespace PortalForgeX.Application.Features.UserGroups;

public record GetUserGroupByIdRequest(int Id) : ICommand<GetUserGroupByIdResponse>
{
    public GetUserGroupByIdResponse NewResponse() => new();
}

internal sealed class GetUserGroupByIdHandler(ILogger<GetUserGroupByIdHandler> logger, IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetUserGroupByIdRequest, GetUserGroupByIdResponse>
{
    private readonly ILogger<GetUserGroupByIdHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<GetUserGroupByIdResponse> Handle(GetUserGroupByIdRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        try
        {
            var result = await _unitOfWork.UserGroupRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                response.SetFailure($"Usergroup with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
                return response;
            }

            response.SetSuccess(_mapper.Map<UserGroupDto>(result));
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, ex.Message);
            response.SetFailure("There was a technical error.", StatusCodes.Status500InternalServerError);
        }

        return response;
    }
}
