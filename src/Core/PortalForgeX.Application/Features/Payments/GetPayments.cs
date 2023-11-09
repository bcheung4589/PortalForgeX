using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Clients;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.DTOs;
using PortalForgeX.Shared.Features.Payments;

namespace PortalForgeX.Application.Features.Payments;

public record GetPaymentsRequest(EntityPageSetting Settings) : ICommand<GetPaymentsResponse>
{
    public GetPaymentsResponse NewResponse() => new();
}

internal sealed class GetPaymentsHandler(ILogger<GetClientsHandler> logger, IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetPaymentsRequest, GetPaymentsResponse>
{
    private readonly ILogger<GetClientsHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<GetPaymentsResponse> Handle(GetPaymentsRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        try
        {
            var result = await _unitOfWork.PaymentRepository.GetPageAsync(request.Settings, cancellationToken);

            response.SetSuccess(_mapper.Map<PagedList<PaymentDto>>(result));
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, ex.Message);
            response.SetFailure("There was a technical error.", StatusCodes.Status500InternalServerError);
        }

        return response;
    }
}
