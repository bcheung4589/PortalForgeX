using FluentValidation;

namespace PortalForgeX.Shared.Features.UserGroups;

public class UserGroupValidation : AbstractValidator<UserGroupDto>
{
    public UserGroupValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(UserGroupResources.Name_NotEmpty)
            .MaximumLength(100).WithMessage(UserGroupResources.Name_MaximumLength);

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage(UserGroupResources.Description_MaximumLength);
    }
}
