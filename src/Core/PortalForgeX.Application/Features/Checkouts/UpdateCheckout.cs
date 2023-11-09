using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.Checkouts;

namespace PortalForgeX.Application.Features.Checkouts;

public record UpdateCheckoutRequest(int Id, CheckoutDto Checkout) : ICommand<UpdateCheckoutResponse>
{
    public UpdateCheckoutResponse NewResponse() => new();
}

internal sealed class UpdateCheckoutHandler(
    ILogger<UpdateCheckoutHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    UpdateCheckoutValidation validator)
    : ValidationHandlerBase<UpdateCheckoutRequest, UpdateCheckoutResponse>(validator)
    , IRequestHandler<UpdateCheckoutRequest, UpdateCheckoutResponse>
{
    private readonly ILogger<UpdateCheckoutHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<UpdateCheckoutResponse> Handle(UpdateCheckoutRequest request, CancellationToken cancellationToken)
    {
        var response = await ValidateRequestAsync(request, cancellationToken);
        if (response.HasError)
        {
            return response;
        }

        try
        {
            var foundObject = await _unitOfWork.CheckoutRepository.GetByIdAsync(request.Id, cancellationToken);
            if (foundObject is null)
            {
                response.SetFailure($"Checkout with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
                return response;
            }

            _mapper.Map(request.Checkout, foundObject);

            var resultObject = await _unitOfWork.CheckoutRepository.UpdateAsync(foundObject, cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (result == 0)
            {
                response.SetFailure("Failed updating checkout.");
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

public class UpdateCheckoutValidation : AbstractValidator<UpdateCheckoutRequest>
{
    public UpdateCheckoutValidation()
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
