using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.Payments;

namespace PortalForgeX.Application.Features.Payments;

public record DeletePaymentRequest(Guid Id) : ICommand<DeletePaymentResponse>
{
    public DeletePaymentResponse NewResponse() => new();
}

internal sealed class DeletePaymentHandler(ILogger<DeletePaymentHandler> logger, IUnitOfWork unitOfWork)
    : IRequestHandler<DeletePaymentRequest, DeletePaymentResponse>
{
    private readonly ILogger<DeletePaymentHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<DeletePaymentResponse> Handle(DeletePaymentRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        try
        {
            var foundObject = await _unitOfWork.PaymentRepository.GetByIdAsync(request.Id, cancellationToken);
            if (foundObject is null)
            {
                response.SetFailure($"Payment with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
                return response;
            }

            _ = await _unitOfWork.PaymentRepository.DeleteAsync(foundObject, cancellationToken);
            _ = await _unitOfWork.SaveChangesAsync(cancellationToken);

            response.SetSuccess();
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, ex.Message);
            response.SetFailure("There was a technical error.", StatusCodes.Status500InternalServerError);
        }

        return response;
    }
}
