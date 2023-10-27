using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.Clients;

namespace PortalForgeX.Application.Features.Clients;

public record UpdateClientRequest(ClientDto Client, Guid Id) : ICommand<UpdateClientResponse>
{
    public UpdateClientResponse NewResponse()
        => new();
}

internal sealed class UpdateClientHandler : IRequestHandler<UpdateClientRequest, UpdateClientResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateClientHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UpdateClientResponse> Handle(UpdateClientRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        // work
        var foundObject = await _unitOfWork.ClientRepository.GetByIdAsync(request.Id, cancellationToken);
        if (foundObject is null)
        {
            response.SetFailure($"Client with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
            return response;
        }

        _mapper.Map(request.Client, foundObject);

        var resultObject = await _unitOfWork.ClientRepository.UpdateAsync(foundObject, cancellationToken);
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (result == 0)
        {
            response.SetFailure("Failed updating client.");
            return response;
        }

        // process
        response.SetSuccess(_mapper.Map<ClientDto>(resultObject));

        // return
        return response;
    }
}
