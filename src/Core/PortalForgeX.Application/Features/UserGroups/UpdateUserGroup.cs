using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.UserGroups;

namespace PortalForgeX.Application.Features.UserGroups;

public record UpdateUserGroupRequest(int Id, UserGroupDto UserGroup) : ICommand<UpdateUserGroupResponse>
{
    public UpdateUserGroupResponse NewResponse() => new();
}

internal sealed class UpdateUserGroupHandler(
    ILogger<UpdateUserGroupHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    UpdateUserGroupValidation validator)
    : ValidationHandlerBase<UpdateUserGroupRequest, UpdateUserGroupResponse>(validator)
    , IRequestHandler<UpdateUserGroupRequest, UpdateUserGroupResponse>
{
    private readonly ILogger<UpdateUserGroupHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<UpdateUserGroupResponse> Handle(UpdateUserGroupRequest request, CancellationToken cancellationToken)
    {
        var response = await ValidateRequestAsync(request, cancellationToken);
        if (response.HasError)
        {
            return response;
        }

        try
        {
            var foundObject = await _unitOfWork.UserGroupRepository.GetByIdAsync(request.Id, cancellationToken);
            if (foundObject is null)
            {
                response.SetFailure($"UserGroup with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
                return response;
            }

            _mapper.Map(request.UserGroup, foundObject);

            var resultObject = await _unitOfWork.UserGroupRepository.UpdateAsync(foundObject, cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (result == 0)
            {
                response.SetFailure("Failed updating UserGroup.");
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

public class UpdateUserGroupValidation : AbstractValidator<UpdateUserGroupRequest>
{
    public UpdateUserGroupValidation()
    {
        RuleFor(x => x.UserGroup.Name)
            .NotEmpty().WithMessage("Name is a required field.")
            .MaximumLength(100).WithMessage("Name can have max 100 chars.");

        RuleFor(x => x.UserGroup.Description)
            .MaximumLength(2000).WithMessage("Description can have max 2000 chars.");
    }
}
