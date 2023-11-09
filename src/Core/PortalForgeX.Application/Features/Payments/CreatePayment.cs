using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Domain.Entities;
using PortalForgeX.Shared.Features.Payments;

namespace PortalForgeX.Application.Features.Payments;

public record CreatePaymentRequest(PaymentDto Payment) : ICommand<CreatePaymentResponse>
{
    public CreatePaymentResponse NewResponse() => new();
}

internal sealed class CreatePaymentHandler(
    ILogger<CreatePaymentHandler> logger,
    IUnitOfWork unitOfWork, 
    IMapper mapper,
    CreatePaymentValidation validator)
    : ValidationHandlerBase<CreatePaymentRequest, CreatePaymentResponse>(validator)
    , IRequestHandler<CreatePaymentRequest, CreatePaymentResponse>
{
    private readonly ILogger<CreatePaymentHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<CreatePaymentResponse> Handle(CreatePaymentRequest request, CancellationToken cancellationToken)
    {
        var response = await ValidateRequestAsync(request, cancellationToken);
        if (response.HasError)
        {
            return response;
        }

        try
        {
            var resultObject = await _unitOfWork.PaymentRepository.InsertAsync(_mapper.Map<Payment>(request.Payment), cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (result == 0)
            {
                response.SetFailure("Failed adding payment.");
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

public class CreatePaymentValidation : AbstractValidator<CreatePaymentRequest>
{
    public CreatePaymentValidation()
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
