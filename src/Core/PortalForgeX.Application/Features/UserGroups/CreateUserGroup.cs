using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Domain.Entities;
using PortalForgeX.Shared.Features.UserGroups;

namespace PortalForgeX.Application.Features.UserGroups;

public record CreateUserGroupRequest(UserGroupDto UserGroup) : ICommand<CreateUserGroupResponse>
{
    public CreateUserGroupResponse NewResponse() => new();
}

internal sealed class CreateUserGroupHandler(
    ILogger<CreateUserGroupHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    CreateUserGroupValidation validator)
    : ValidationHandlerBase<CreateUserGroupRequest, CreateUserGroupResponse>(validator)
    , IRequestHandler<CreateUserGroupRequest, CreateUserGroupResponse>
{
    private readonly ILogger<CreateUserGroupHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<CreateUserGroupResponse> Handle(CreateUserGroupRequest request, CancellationToken cancellationToken)
    {
        var response = await ValidateRequestAsync(request, cancellationToken);
        if (response.HasError)
        {
            return response;
        }

        try
        {
            var resultObject = await _unitOfWork.UserGroupRepository.InsertAsync(_mapper.Map<UserGroup>(request.UserGroup), cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (result == 0)
            {
                response.SetFailure($"Failed adding client {request.UserGroup.Name}.");
                return response;
            }

            response.SetSuccess(_mapper.Map<UserGroupDto>(resultObject));
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, ex.Message);
            response.SetFailure("There was a technical error.", StatusCodes.Status500InternalServerError);
        }

        return response;
    }
}

public class CreateUserGroupValidation : AbstractValidator<CreateUserGroupRequest>
{
    public CreateUserGroupValidation()
    {
        RuleFor(x => x.UserGroup.Name)
            .NotEmpty().WithMessage("Name is a required field.")
            .MaximumLength(100).WithMessage("Name can have max 100 chars.");

        RuleFor(x => x.UserGroup.Description)
            .MaximumLength(2000).WithMessage("Description can have max 2000 chars.");
    }
}
