using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Clients;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.Checkouts;

namespace PortalForgeX.Application.Features.Checkouts;

public record GetCheckoutByIdRequest(int Id) : ICommand<GetCheckoutByIdResponse>
{
    public GetCheckoutByIdResponse NewResponse() => new();
}

internal sealed class GetCheckoutByIdHandler(ILogger<GetClientByIdHandler> logger, IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetCheckoutByIdRequest, GetCheckoutByIdResponse>
{
    private readonly ILogger<GetClientByIdHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<GetCheckoutByIdResponse> Handle(GetCheckoutByIdRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        try
        {
            var result = await _unitOfWork.CheckoutRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                response.SetFailure($"Checkout with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
                return response;
            }

            response.SetSuccess(_mapper.Map<CheckoutDto>(result));
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, ex.Message);
            response.SetFailure("There was a technical error.", StatusCodes.Status500InternalServerError);
        }

        return response;
    }
}
