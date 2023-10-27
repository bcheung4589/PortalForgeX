using AutoMapper;
using MediatR;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Domain.Entities;
using PortalForgeX.Shared.Features.ClientContacts;

namespace PortalForgeX.Application.Features.ClientContacts;

public record CreateClientContactRequest(ClientContactDto ClientContact) : ICommand<CreateClientContactResponse>
{
    public CreateClientContactResponse NewResponse()
        => new();
}

internal sealed class CreateClientContactHandler : IRequestHandler<CreateClientContactRequest, CreateClientContactResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateClientContactHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateClientContactResponse> Handle(CreateClientContactRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        // work
        var test = _mapper.Map<ClientContact>(request.ClientContact);
        var resultObject = await _unitOfWork.ClientContactRepository.InsertAsync(test, cancellationToken);
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (result == 0)
        {
            response.SetFailure("Failed adding client contactperson.");
            return response;
        }

        // process
        response.SetSuccess(_mapper.Map<ClientContactDto>(resultObject));

        // return
        return response;
    }
}
