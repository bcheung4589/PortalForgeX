using FluentValidation;

namespace PortalForgeX.Shared.Features.Clients;

public class ClientValidation : AbstractValidator<ClientDto>
{
    public ClientValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ClientResources.Name_NotEmpty)
            .MaximumLength(100).WithMessage(ClientResources.Name_MaximumLength);

        RuleFor(x => x.Remarks)
            .MaximumLength(2000).WithMessage(ClientResources.Remarks_MaximumLength);
    }
}
