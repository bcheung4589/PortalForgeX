using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.ClientContacts;

namespace PortalForgeX.Application.Features.ClientContacts;

public record GetClientContactByIdRequest(Guid Id) : ICommand<GetClientContactByIdResponse>
{
    public GetClientContactByIdResponse NewResponse()
        => new();
}

internal sealed class GetClientContactByIdHandler : IRequestHandler<GetClientContactByIdRequest, GetClientContactByIdResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetClientContactByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetClientContactByIdResponse> Handle(GetClientContactByIdRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        // work
        var result = await _unitOfWork.ClientContactRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            response.SetFailure($"Client contactperson with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
            return response;
        }

        // process
        response.SetSuccess(_mapper.Map<ClientContactDto>(result));

        // return
        return response;
    }
}
