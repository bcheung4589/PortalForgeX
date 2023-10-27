using MediatR;
using Microsoft.AspNetCore.Http;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.Clients;

namespace PortalForgeX.Application.Features.Clients;

public record DeleteClientRequest(Guid Id) : ICommand<DeleteClientResponse>
{
    public DeleteClientResponse NewResponse()
        => new();
}

internal sealed class DeleteClientHandler : IRequestHandler<DeleteClientRequest, DeleteClientResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteClientHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteClientResponse> Handle(DeleteClientRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        // work
        var foundObject = await _unitOfWork.ClientRepository.GetByIdAsync(request.Id, cancellationToken);
        if (foundObject is null)
        {
            response.SetFailure($"Client with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
            return response;
        }

        _ = await _unitOfWork.ClientRepository.DeleteAsync(foundObject, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // process
        response.SetSuccess();

        // return
        return response;
    }
}
