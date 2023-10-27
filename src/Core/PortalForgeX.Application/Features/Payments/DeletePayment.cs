using MediatR;
using Microsoft.AspNetCore.Http;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.Payments;

namespace PortalForgeX.Application.Features.Payments;

public record DeletePaymentRequest(Guid Id) : ICommand<DeletePaymentResponse>
{
    public DeletePaymentResponse NewResponse()
        => new();
}

internal sealed class DeletePaymentHandler : IRequestHandler<DeletePaymentRequest, DeletePaymentResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePaymentHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<DeletePaymentResponse> Handle(DeletePaymentRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        // work
        var foundObject = await _unitOfWork.PaymentRepository.GetByIdAsync(request.Id, cancellationToken);
        if (foundObject is null)
        {
            response.SetFailure($"Payment with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
            return response;
        }

        _ = await _unitOfWork.PaymentRepository.DeleteAsync(foundObject, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // process
        response.SetSuccess();

        // return
        return response;
    }
}
