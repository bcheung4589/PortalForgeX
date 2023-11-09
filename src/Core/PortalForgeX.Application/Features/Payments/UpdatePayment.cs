using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.Payments;

namespace PortalForgeX.Application.Features.Payments;

public record UpdatePaymentRequest(Guid Id, PaymentDto Payment) : ICommand<UpdatePaymentResponse>
{
    public UpdatePaymentResponse NewResponse() => new();
}

internal sealed class UpdatePaymentHandler(
    ILogger<UpdatePaymentHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    UpdatePaymentValidation validator)
    : ValidationHandlerBase<UpdatePaymentRequest, UpdatePaymentResponse>(validator)
    , IRequestHandler<UpdatePaymentRequest, UpdatePaymentResponse>
{
    private readonly ILogger<UpdatePaymentHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<UpdatePaymentResponse> Handle(UpdatePaymentRequest request, CancellationToken cancellationToken)
    {
        var response = await ValidateRequestAsync(request, cancellationToken);
        if (response.HasError)
        {
            return response;
        }

        try
        {
            var foundObject = await _unitOfWork.PaymentRepository.GetByIdAsync(request.Id, cancellationToken);
            if (foundObject is null)
            {
                response.SetFailure($"Payment with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
                return response;
            }

            _mapper.Map(request.Payment, foundObject);

            var resultObject = await _unitOfWork.PaymentRepository.UpdateAsync(foundObject, cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (result == 0)
            {
                response.SetFailure("Failed updating payment.");
                return response;
            }

            response.SetSuccess(_mapper.Map<PaymentDto>(resultObject));
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, ex.Message);
            response.SetFailure("There was a technical error.", StatusCodes.Status500InternalServerError);
        }

        return response;
    }
}

public class UpdatePaymentValidation : AbstractValidator<UpdatePaymentRequest>
{
    public UpdatePaymentValidation()
    {
        RuleFor(x => x.Payment.Amount)
            .GreaterThan(0).WithMessage("Amount should be a positive number.");

        RuleFor(x => x.Payment.PaymentMethod)
            .NotEmpty().WithMessage("Payment Method is a required field.")
            .MaximumLength(50).WithMessage("Payment Method can have max 50 chars.");

        RuleFor(x => x.Payment.TransactionId)
            .MaximumLength(100).WithMessage("Transaction Id can have max 100 chars.");
    }
}
