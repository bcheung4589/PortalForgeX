using FluentValidation;

namespace PortalForgeX.Shared.Features.Tenants;

public class TenantValidation : AbstractValidator<TenantFormModel>
{
    public TenantValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(TenantResources.Name_NotEmpty)
            .MaximumLength(100).WithMessage(TenantResources.Name_MaximumLength);

        RuleFor(x => x.Host)
            .NotEmpty().WithMessage(TenantResources.Host_NotEmpty)
            .MaximumLength(100).WithMessage(TenantResources.Host_MaximumLength);

        RuleFor(x => x.Remarks)
            .MaximumLength(2000).WithMessage(TenantResources.Remarks_MaximumLength);
    }
}
