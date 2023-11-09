using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.DTOs;
using PortalForgeX.Shared.Features.BusinessLocations;

namespace PortalForgeX.Application.Features.BusinessLocations;

public record GetBusinessLocationsRequest(EntityPageSetting Settings) : ICommand<GetBusinessLocationsResponse>
{
    public GetBusinessLocationsResponse NewResponse() => new();
}

internal sealed class GetBusinessLocationsHandler(ILogger<GetBusinessLocationsHandler> logger, IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetBusinessLocationsRequest, GetBusinessLocationsResponse>
{
    private readonly ILogger<GetBusinessLocationsHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<GetBusinessLocationsResponse> Handle(GetBusinessLocationsRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        try
        {
            var result = await _unitOfWork.BusinessLocationRepository.GetPageAsync(request.Settings, cancellationToken);

            response.SetSuccess(_mapper.Map<PagedList<BusinessLocationDto>>(result));
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, ex.Message);
            response.SetFailure("There was a technical error.", StatusCodes.Status500InternalServerError);
        }

        return response;
    }
}
