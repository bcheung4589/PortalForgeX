using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.DTOs;
using PortalForgeX.Shared.Features.Clients;

namespace PortalForgeX.Application.Features.Clients;

public record GetClientsRequest(EntityPageSetting Settings) : ICommand<GetClientsResponse>
{
    public GetClientsResponse NewResponse() => new();
}

internal sealed class GetClientsHandler(ILogger<GetClientsHandler> logger, IUnitOfWork unitOfWork, IMapper mapper) 
    : IRequestHandler<GetClientsRequest, GetClientsResponse>
{
    private readonly ILogger<GetClientsHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<GetClientsResponse> Handle(GetClientsRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        try
        {
            var result = await _unitOfWork.ClientRepository.GetPageAsync(request.Settings, cancellationToken);

            response.SetSuccess(_mapper.Map<PagedList<ClientDto>>(result));
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, ex.Message);
            response.SetFailure("There was a technical error.", StatusCodes.Status500InternalServerError);
        }

        return response;
    }
}
