using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.Payments;

namespace PortalForgeX.Application.Features.Payments;

public record UpdatePaymentRequest(Guid Id, PaymentDto Payment) : ICommand<UpdatePaymentResponse>
{
    public UpdatePaymentResponse NewResponse()
        => new();
}

internal sealed class UpdatePaymentHandler : IRequestHandler<UpdatePaymentRequest, UpdatePaymentResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePaymentHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UpdatePaymentResponse> Handle(UpdatePaymentRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        // work
        var foundObject = await _unitOfWork.PaymentRepository.GetByIdAsync(request.Id, cancellationToken);
        if (foundObject is null)
        {
            response.SetFailure($"Payment with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
            return response;
        }

        _mapper.Map(request.Payment, foundObject);

        var resultObject = await _unitOfWork.PaymentRepository.UpdateAsync(foundObject, cancellationToken);
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (result == 0)
        {
            response.SetFailure("Failed updating payment.");
            return response;
        }

        // process
        response.SetSuccess(_mapper.Map<PaymentDto>(resultObject));

        // return
        return response;
    }
}