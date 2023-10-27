using MediatR;
using Microsoft.AspNetCore.Http;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.Checkouts;

namespace PortalForgeX.Application.Features.Checkouts;

public record DeleteCheckoutRequest(int Id) : ICommand<DeleteCheckoutResponse>
{
    public DeleteCheckoutResponse NewResponse()
        => new();
}

internal sealed class DeleteCheckoutHandler : IRequestHandler<DeleteCheckoutRequest, DeleteCheckoutResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCheckoutHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteCheckoutResponse> Handle(DeleteCheckoutRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        // work
        var foundObject = await _unitOfWork.CheckoutRepository.GetByIdAsync(request.Id, cancellationToken);
        if (foundObject is null)
        {
            response.SetFailure($"Checkout with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
            return response;
        }

        _ = await _unitOfWork.CheckoutRepository.DeleteAsync(foundObject, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // process
        response.SetSuccess();

        // return
        return response;
    }
}
