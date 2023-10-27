using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.BusinessLocations;

namespace PortalForgeX.Application.Features.BusinessLocations;

public record GetBusinessLocationByIdRequest(int Id) : ICommand<GetBusinessLocationByIdResponse>
{
    public GetBusinessLocationByIdResponse NewResponse()
        => new();
}

internal sealed class GetBusinessLocationByIdHandler : IRequestHandler<GetBusinessLocationByIdRequest, GetBusinessLocationByIdResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBusinessLocationByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetBusinessLocationByIdResponse> Handle(GetBusinessLocationByIdRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        // work
        var result = await _unitOfWork.BusinessLocationRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            response.SetFailure($"Business location with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
            return response;
        }

        // process
        response.SetSuccess(_mapper.Map<BusinessLocationDto>(result));

        // return
        return response;
    }
}
