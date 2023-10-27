using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.Checkouts;

namespace PortalForgeX.Application.Features.Checkouts;

public record UpdateCheckoutRequest(int Id, CheckoutDto Checkout) : ICommand<UpdateCheckoutResponse>
{
    public UpdateCheckoutResponse NewResponse()
        => new();
}

internal sealed class UpdateCheckoutHandler : IRequestHandler<UpdateCheckoutRequest, UpdateCheckoutResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCheckoutHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UpdateCheckoutResponse> Handle(UpdateCheckoutRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        // work
        var foundObject = await _unitOfWork.CheckoutRepository.GetByIdAsync(request.Id, cancellationToken);
        if (foundObject is null)
        {
            response.SetFailure($"Checkout with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
            return response;
        }

        _mapper.Map(request.Checkout, foundObject);

        var resultObject = await _unitOfWork.CheckoutRepository.UpdateAsync(foundObject, cancellationToken);
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (result == 0)
        {
            response.SetFailure("Failed updating checkout.");
            return response;
        }

        // process
        response.SetSuccess(_mapper.Map<CheckoutDto>(resultObject));

        // return
        return response;
    }
}
