using AutoMapper;
using MediatR;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Domain.Entities;
using PortalForgeX.Shared.Features.Payments;

namespace PortalForgeX.Application.Features.Payments;

public record CreatePaymentRequest(PaymentDto Payment) : ICommand<CreatePaymentResponse>
{
    public CreatePaymentResponse NewResponse()
        => new();
}

internal sealed class CreatePaymentHandler : IRequestHandler<CreatePaymentRequest, CreatePaymentResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePaymentHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreatePaymentResponse> Handle(CreatePaymentRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        // work
        var resultObject = await _unitOfWork.PaymentRepository.InsertAsync(_mapper.Map<Payment>(request.Payment), cancellationToken);
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (result == 0)
        {
            response.SetFailure("Failed adding payment.");
            return response;
        }

        // process
        response.SetSuccess(_mapper.Map<PaymentDto>(resultObject));

        // return
        return response;
    }
}
