using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.BusinessLocations;

namespace PortalForgeX.Application.Features.BusinessLocations;

public record DeleteBusinessLocationRequest(int Id) : ICommand<DeleteBusinessLocationResponse>
{
    public DeleteBusinessLocationResponse NewResponse()
        => new();
}

internal sealed class DeleteBusinessLocationHandler : IRequestHandler<DeleteBusinessLocationRequest, DeleteBusinessLocationResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBusinessLocationHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteBusinessLocationResponse> Handle(DeleteBusinessLocationRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        // work
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
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // process
        response.SetSuccess();

        // return
        return response;
    }
}
