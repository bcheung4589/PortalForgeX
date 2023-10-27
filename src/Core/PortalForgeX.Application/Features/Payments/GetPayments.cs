using AutoMapper;
using MediatR;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.DTOs;
using PortalForgeX.Shared.Features.Payments;

namespace PortalForgeX.Application.Features.Payments;

public record GetPaymentsRequest(EntityPageSetting Settings) : ICommand<GetPaymentsResponse>
{
    public GetPaymentsResponse NewResponse()
        => new();
}

internal sealed class GetPaymentsHandler : IRequestHandler<GetPaymentsRequest, GetPaymentsResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPaymentsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetPaymentsResponse> Handle(GetPaymentsRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        // work
        var result = await _unitOfWork.PaymentRepository.GetPageAsync(request.Settings, cancellationToken);

        // process
        response.SetSuccess(_mapper.Map<PageModel<PaymentDto>>(result));

        // return
        return response;
    }
}
