using FluentValidation;

namespace PortalForgeX.Shared.Features.BusinessLocations
{
    public class BusinessLocationValidation : AbstractValidator<BusinessLocationDto>
    {
        public BusinessLocationValidation()
        {
            RuleFor(x => x.ApiKey)
                .MaximumLength(100).WithMessage(BusinessLocationResources.ApiKey_MaximumLength);

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage(BusinessLocationResources.Street_NotEmpty)
                .MaximumLength(200).WithMessage(BusinessLocationResources.Street_MaximumLength);

            RuleFor(x => x.HouseNr)
                .NotEmpty().WithMessage(BusinessLocationResources.HouseNr_NotEmpty)
                .MaximumLength(10).WithMessage(BusinessLocationResources.HouseNr_MaximumLength);

            RuleFor(x => x.Zipcode)
                .NotEmpty().WithMessage(BusinessLocationResources.Zipcode_NotEmpty)
                .MaximumLength(10).WithMessage(BusinessLocationResources.Zipcode_MaximumLength);

            RuleFor(x => x.Place)
                .NotEmpty().WithMessage(BusinessLocationResources.Place_NotEmpty)
                .MaximumLength(100).WithMessage(BusinessLocationResources.Place_MaximumLength);

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage(BusinessLocationResources.Country_NotEmpty)
                .MaximumLength(100).WithMessage(BusinessLocationResources.ApiKey_MaximumLength);

            RuleFor(x => x.IpAddress)
                .MaximumLength(50).WithMessage(BusinessLocationResources.IpAddress_MaximumLength);

            RuleFor(x => x.Remarks)
                .MaximumLength(2000).WithMessage(BusinessLocationResources.Remarks_MaximumLength);
        }
    }
}
