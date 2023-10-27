using AutoMapper;
using MediatR;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Domain.Entities;
using PortalForgeX.Shared.Features.Clients;

namespace PortalForgeX.Application.Features.Clients;

public record CreateClientRequest(ClientDto Client) : ICommand<CreateClientResponse>
{
    public CreateClientResponse NewResponse()
        => new();
}

internal sealed class CreateClientHandler : IRequestHandler<CreateClientRequest, CreateClientResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateClientHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateClientResponse> Handle(CreateClientRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        // work
        var resultObject = await _unitOfWork.ClientRepository.InsertAsync(_mapper.Map<Client>(request.Client), cancellationToken);
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (result == 0)
        {
            response.SetFailure("Failed adding client.");
            return response;
        }

        // process
        response.SetSuccess(_mapper.Map<ClientDto>(resultObject));

        // return
        return response;
    }
}
