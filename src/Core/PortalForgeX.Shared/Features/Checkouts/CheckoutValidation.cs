using FluentValidation;

namespace PortalForgeX.Shared.Features.Checkouts;

public class CheckoutValidation : AbstractValidator<CheckoutDto>
{
    public CheckoutValidation()
    {
        RuleFor(x => x.DeviceCode)
            .NotEmpty().WithMessage(CheckoutResources.DeviceCode_NotEmpty)
            .MaximumLength(50).WithMessage(CheckoutResources.DeviceCode_MaximumLength);

        RuleFor(x => x.SoftwareVersion)
            .NotEmpty().WithMessage(CheckoutResources.SoftwareVersion_NotEmpty)
            .MaximumLength(10).WithMessage(CheckoutResources.SoftwareVersion_MaximumLength);

        RuleFor(x => x.Remarks)
            .MaximumLength(2000).WithMessage(CheckoutResources.Remarks_MaximumLength);
    }
}
