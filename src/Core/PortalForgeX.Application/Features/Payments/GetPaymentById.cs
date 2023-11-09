using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Clients;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.Payments;

namespace PortalForgeX.Application.Features.Payments;

public record GetPaymentByIdRequest(Guid Id) : ICommand<GetPaymentByIdResponse>
{
    public GetPaymentByIdResponse NewResponse() => new();
}

internal sealed class GetPaymentByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetClientsHandler> logger)
    : IRequestHandler<GetPaymentByIdRequest, GetPaymentByIdResponse>
{
    private readonly ILogger<GetClientsHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<GetPaymentByIdResponse> Handle(GetPaymentByIdRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        try
        {
            var result = await _unitOfWork.PaymentRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                response.SetFailure($"Payment with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
                return response;
            }

            response.SetSuccess(_mapper.Map<PaymentDto>(result));
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, ex.Message);
            response.SetFailure("There was a technical error.", StatusCodes.Status500InternalServerError);
        }

        return response;
    }
}
