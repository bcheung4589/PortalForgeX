using AutoMapper;
using MediatR;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.DTOs;
using PortalForgeX.Shared.Features.Clients;

namespace PortalForgeX.Application.Features.Clients;

public record GetClientsRequest(EntityPageSetting Settings) : ICommand<GetClientsResponse>
{
    public GetClientsResponse NewResponse()
        => new();
}

internal sealed class GetClientsHandler : IRequestHandler<GetClientsRequest, GetClientsResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetClientsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetClientsResponse> Handle(GetClientsRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        // work
        var result = await _unitOfWork.ClientRepository.GetPageAsync(request.Settings, cancellationToken);

        // process
        response.SetSuccess(_mapper.Map<PagedList<ClientDto>>(result));

        // return
        return response;
    }
}
