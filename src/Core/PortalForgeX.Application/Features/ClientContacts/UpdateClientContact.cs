using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.ClientContacts;

namespace PortalForgeX.Application.Features.ClientContacts;

public record UpdateClientContactRequest(Guid Id, ClientContactDto ClientContact) : ICommand<UpdateClientContactResponse>
{
    public UpdateClientContactResponse NewResponse()
        => new();
}

internal sealed class UpdateClientContactHandler : IRequestHandler<UpdateClientContactRequest, UpdateClientContactResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateClientContactHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UpdateClientContactResponse> Handle(UpdateClientContactRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        // work
        var foundObject = await _unitOfWork.ClientContactRepository.GetByIdAsync(request.Id, cancellationToken);
        if (foundObject is null)
        {
            response.SetFailure($"Client contactperson with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
            return response;
        }

        _mapper.Map(request.ClientContact, foundObject);

        var resultObject = await _unitOfWork.ClientContactRepository.UpdateAsync(foundObject, cancellationToken);
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (result == 0)
        {
            response.SetFailure("Failed updating client contactperson.");
            return response;
        }

        // process
        response.SetSuccess(_mapper.Map<ClientContactDto>(resultObject));

        // return
        return response;
    }
}
