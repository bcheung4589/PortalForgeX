using FluentValidation;

namespace PortalForgeX.Shared.Features.Payments;

public class PaymentValidation : AbstractValidator<PaymentDto>
{
    public PaymentValidation()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage(PaymentResources.Amount_GreaterThan);

        RuleFor(x => x.PaymentMethod)
            .NotEmpty().WithMessage(PaymentResources.PaymentMethod_NotEmpty)
            .MaximumLength(50).WithMessage(PaymentResources.PaymentMethod_MaximumLength);

        RuleFor(x => x.TransactionId)
            .MaximumLength(100).WithMessage(PaymentResources.TransactionId_MaximumLength);
    }
}
