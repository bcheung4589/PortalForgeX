using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.BusinessLocations;

namespace PortalForgeX.Application.Features.BusinessLocations;

public record UpdateBusinessLocationRequest(int Id, BusinessLocationDto BusinessLocation) : ICommand<UpdateBusinessLocationResponse>
{
    public UpdateBusinessLocationResponse NewResponse()
        => new();
}

internal sealed class UpdateBusinessLocationHandler : IRequestHandler<UpdateBusinessLocationRequest, UpdateBusinessLocationResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateBusinessLocationHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UpdateBusinessLocationResponse> Handle(UpdateBusinessLocationRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        // work
        var foundObject = await _unitOfWork.BusinessLocationRepository.GetByIdAsync(request.Id, cancellationToken);
        if (foundObject is null)
        {
            response.SetFailure($"Business location with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
            return response;
        }

        _mapper.Map(request.BusinessLocation, foundObject);

        var resultObject = await _unitOfWork.BusinessLocationRepository.UpdateAsync(foundObject, cancellationToken);
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (result == 0)
        {
            response.SetFailure("Failed updating business location.");
            return response;
        }

        // process
        response.SetSuccess(_mapper.Map<BusinessLocationDto>(resultObject));

        // return
        return response;
    }
}
