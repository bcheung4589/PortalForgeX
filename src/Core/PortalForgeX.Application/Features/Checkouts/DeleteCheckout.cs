using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.Checkouts;

namespace PortalForgeX.Application.Features.Checkouts;

public record DeleteCheckoutRequest(int Id) : ICommand<DeleteCheckoutResponse>
{
    public DeleteCheckoutResponse NewResponse() => new();
}

internal sealed class DeleteCheckoutHandler(ILogger<DeleteCheckoutHandler> logger, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteCheckoutRequest, DeleteCheckoutResponse>
{
    private readonly ILogger<DeleteCheckoutHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<DeleteCheckoutResponse> Handle(DeleteCheckoutRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        try
        {
            var foundObject = await _unitOfWork.CheckoutRepository.GetByIdAsync(request.Id, cancellationToken);
            if (foundObject is null)
            {
                response.SetFailure($"Checkout with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
                return response;
            }

            _ = await _unitOfWork.CheckoutRepository.DeleteAsync(foundObject, cancellationToken);
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
