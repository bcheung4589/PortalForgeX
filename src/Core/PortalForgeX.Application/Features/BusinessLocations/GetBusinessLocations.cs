using AutoMapper;
using MediatR;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.DTOs;
using PortalForgeX.Shared.Features.BusinessLocations;

namespace PortalForgeX.Application.Features.BusinessLocations;

public record GetBusinessLocationsRequest(EntityPageSetting Settings) : ICommand<GetBusinessLocationsResponse>
{
    public GetBusinessLocationsResponse NewResponse()
        => new();
}

internal sealed class GetBusinessLocationsHandler : IRequestHandler<GetBusinessLocationsRequest, GetBusinessLocationsResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBusinessLocationsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetBusinessLocationsResponse> Handle(GetBusinessLocationsRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        // work
        var result = await _unitOfWork.BusinessLocationRepository.GetPageAsync(request.Settings, cancellationToken);

        // process
        response.SetSuccess(_mapper.Map<PagedList<BusinessLocationDto>>(result));

        // return
        return response;
    }
}
