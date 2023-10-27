using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.Clients;

namespace PortalForgeX.Application.Features.Clients;

public record GetClientByIdRequest(Guid Id) : ICommand<GetClientByIdResponse>
{
    public GetClientByIdResponse NewResponse()
        => new();
}

internal sealed class GetClientByIdHandler : IRequestHandler<GetClientByIdRequest, GetClientByIdResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetClientByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetClientByIdResponse> Handle(GetClientByIdRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        // work
        var result = await _unitOfWork.ClientRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            response.SetFailure($"Client with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
            return response;
        }

        // process
        response.SetSuccess(_mapper.Map<ClientDto>(result));

        // return
        return response;
    }
}
