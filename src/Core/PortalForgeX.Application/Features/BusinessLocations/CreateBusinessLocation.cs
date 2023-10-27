using AutoMapper;
using MediatR;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Domain.Entities;
using PortalForgeX.Shared.Features.BusinessLocations;

namespace PortalForgeX.Application.Features.BusinessLocations;

public record CreateBusinessLocationRequest(BusinessLocationDto BusinessLocation) : ICommand<CreateBusinessLocationResponse>
{
    public CreateBusinessLocationResponse NewResponse()
        => new();
}

internal sealed class CreateBusinessLocationHandler : IRequestHandler<CreateBusinessLocationRequest, CreateBusinessLocationResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateBusinessLocationHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateBusinessLocationResponse> Handle(CreateBusinessLocationRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        // work
        var resultObject = await _unitOfWork.BusinessLocationRepository.InsertAsync(_mapper.Map<BusinessLocation>(request.BusinessLocation), cancellationToken);
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (result == 0)
        {
            response.SetFailure("Failed adding business location.");
            return response;
        }

        // process
        response.SetSuccess(_mapper.Map<BusinessLocationDto>(resultObject));

        // return
        return response;
    }
}
