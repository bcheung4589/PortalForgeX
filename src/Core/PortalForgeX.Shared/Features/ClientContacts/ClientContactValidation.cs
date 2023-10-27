using FluentValidation;

namespace PortalForgeX.Shared.Features.ClientContacts;

public class ClientContactValidation : AbstractValidator<ClientContactDto>
{
    public ClientContactValidation()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage(ClientContactResources.FullName_NotEmpty)
            .MaximumLength(100).WithMessage(ClientContactResources.FullName_MaximumLength);

        RuleFor(x => x.PhoneNr)
            .NotEmpty().WithMessage(ClientContactResources.PhoneNr_NotEmpty)
            .MaximumLength(20).WithMessage(ClientContactResources.PhoneNr_MaximumLength);

        RuleFor(x => x.Email)
            .MaximumLength(150).WithMessage(ClientContactResources.Email_MaximumLength);

        RuleFor(x => x.Remarks)
            .MaximumLength(2000).WithMessage(ClientContactResources.Remarks_MaximumLength);
    }
}
