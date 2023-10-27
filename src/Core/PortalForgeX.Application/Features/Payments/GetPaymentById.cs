using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.Payments;

namespace PortalForgeX.Application.Features.Payments;

public record GetPaymentByIdRequest(Guid Id) : ICommand<GetPaymentByIdResponse>
{
    public GetPaymentByIdResponse NewResponse()
        => new();
}

internal sealed class GetPaymentByIdHandler : IRequestHandler<GetPaymentByIdRequest, GetPaymentByIdResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPaymentByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetPaymentByIdResponse> Handle(GetPaymentByIdRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        // work
        var result = await _unitOfWork.PaymentRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            response.SetFailure($"Payment with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
            return response;
        }

        // process
        response.SetSuccess(_mapper.Map<PaymentDto>(result));

        // return
        return response;
    }
}
