using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Domain.Entities;
using PortalForgeX.Shared.Features.Checkouts;

namespace PortalForgeX.Application.Features.Checkouts;

public record CreateCheckoutRequest(CheckoutDto Checkout) : ICommand<CreateCheckoutResponse>
{
    public CreateCheckoutResponse NewResponse() => new();
}

internal sealed class CreateCheckoutHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<CreateCheckoutHandler> logger,
    CreateCheckoutValidation validator)
    : ValidationHandlerBase<CreateCheckoutRequest, CreateCheckoutResponse>(validator)
    , IRequestHandler<CreateCheckoutRequest, CreateCheckoutResponse>
{
    private readonly ILogger<CreateCheckoutHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<CreateCheckoutResponse> Handle(CreateCheckoutRequest request, CancellationToken cancellationToken)
    {
        var response = await ValidateRequestAsync(request, cancellationToken);
        if (response.HasError)
        {
            return response;
        }

        try
        {
            var resultObject = await _unitOfWork.CheckoutRepository.InsertAsync(_mapper.Map<Checkout>(request.Checkout), cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (result == 0)
            {
                response.SetFailure("Failed adding checkout.");
                return response;
            }

            response.SetSuccess(_mapper.Map<CheckoutDto>(resultObject));
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, ex.Message);
            response.SetFailure("There was a technical error.", StatusCodes.Status500InternalServerError);
        }

        return response;
    }
}

public class CreateCheckoutValidation : AbstractValidator<CreateCheckoutRequest>
{
    public CreateCheckoutValidation()
    {
        RuleFor(x => x.Checkout.DeviceCode)
            .NotEmpty().WithMessage("Device Code is a required field.")
            .MaximumLength(50).WithMessage("Device Code can have max 50 chars.");

        RuleFor(x => x.Checkout.SoftwareVersion)
            .NotEmpty().WithMessage("Software Version is a required field.")
            .MaximumLength(10).WithMessage("Software Version can have max 10 chars.");

        RuleFor(x => x.Checkout.Remarks)
            .MaximumLength(2000).WithMessage("Remarks can have max 2000 chars.");
    }
}
