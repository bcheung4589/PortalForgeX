using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.Clients;

namespace PortalForgeX.Application.Features.Clients;

public record GetClientByIdRequest(Guid Id) : ICommand<GetClientByIdResponse>
{
    public GetClientByIdResponse NewResponse() => new();
}

internal sealed class GetClientByIdHandler(ILogger<GetClientByIdHandler> logger, IUnitOfWork unitOfWork, IMapper mapper) 
    : IRequestHandler<GetClientByIdRequest, GetClientByIdResponse>
{
    private readonly ILogger<GetClientByIdHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<GetClientByIdResponse> Handle(GetClientByIdRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        try
        {
            var result = await _unitOfWork.ClientRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                response.SetFailure($"Client with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
                return response;
            }

            response.SetSuccess(_mapper.Map<ClientDto>(result));
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, ex.Message);
            response.SetFailure("There was a technical error.", StatusCodes.Status500InternalServerError);
        }

        return response;
    }
}
