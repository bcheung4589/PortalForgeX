using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.BusinessLocations;

namespace PortalForgeX.Application.Features.BusinessLocations;

public record GetBusinessLocationByIdRequest(int Id) : ICommand<GetBusinessLocationByIdResponse>
{
    public GetBusinessLocationByIdResponse NewResponse() => new();
}

internal sealed class GetBusinessLocationByIdHandler(ILogger<GetBusinessLocationByIdHandler> logger, IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetBusinessLocationByIdRequest, GetBusinessLocationByIdResponse>
{
    private readonly ILogger<GetBusinessLocationByIdHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<GetBusinessLocationByIdResponse> Handle(GetBusinessLocationByIdRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        try
        {
            var result = await _unitOfWork.BusinessLocationRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                response.SetFailure($"Business location with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
                return response;
            }

            response.SetSuccess(_mapper.Map<BusinessLocationDto>(result));
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, ex.Message);
            response.SetFailure("There was a technical error.", StatusCodes.Status500InternalServerError);
        }

        return response;
    }
}
