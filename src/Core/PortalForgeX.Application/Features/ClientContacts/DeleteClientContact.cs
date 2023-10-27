using MediatR;
using Microsoft.AspNetCore.Http;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.ClientContacts;

namespace PortalForgeX.Application.Features.ClientContacts;

public record DeleteClientContactRequest(Guid Id) : ICommand<DeleteClientContactResponse>
{
    public DeleteClientContactResponse NewResponse()
        => new();
}

internal sealed class DeleteClientContactHandler : IRequestHandler<DeleteClientContactRequest, DeleteClientContactResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteClientContactHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteClientContactResponse> Handle(DeleteClientContactRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        // work
        var foundObject = await _unitOfWork.ClientContactRepository.GetByIdAsync(request.Id, cancellationToken);
        if (foundObject is null)
        {
            response.SetFailure($"Client contactperson with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
            return response;
        }

        _ = await _unitOfWork.ClientContactRepository.DeleteAsync(foundObject, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // process
        response.SetSuccess();

        // return
        return response;
    }
}
