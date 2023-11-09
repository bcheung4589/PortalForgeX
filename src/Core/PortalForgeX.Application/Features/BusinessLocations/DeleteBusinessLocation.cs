using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Clients;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.BusinessLocations;

namespace PortalForgeX.Application.Features.BusinessLocations;

public record DeleteBusinessLocationRequest(int Id) : ICommand<DeleteBusinessLocationResponse>
{
    public DeleteBusinessLocationResponse NewResponse() => new();
}

internal sealed class DeleteBusinessLocationHandler(ILogger<DeleteClientHandler> logger, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteBusinessLocationRequest, DeleteBusinessLocationResponse>
{
    private readonly ILogger<DeleteClientHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<DeleteBusinessLocationResponse> Handle(DeleteBusinessLocationRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        try
        {
            var foundObject = await _unitOfWork.BusinessLocationRepository.GetByIdAsync(request.Id, cancellationToken);
            if (foundObject is null)
            {
                response.SetFailure($"Business location with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
                return response;
            }

            var locationPayments = await _unitOfWork.PaymentRepository.GetAsQuery().Where(x => x.BusinessLocationId == foundObject.Id).ToListAsync(cancellationToken: cancellationToken);
            if (locationPayments is not null && locationPayments.Count != 0)
            {
                response.SetFailure("Cant delete location when there are still payments.", StatusCodes.Status400BadRequest);
                return response;
            }

            _ = await _unitOfWork.BusinessLocationRepository.DeleteAsync(foundObject, cancellationToken);
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
