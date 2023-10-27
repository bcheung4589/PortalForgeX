using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.Checkouts;

namespace PortalForgeX.Application.Features.Checkouts;

public record GetCheckoutByIdRequest(int Id) : ICommand<GetCheckoutByIdResponse>
{
    public GetCheckoutByIdResponse NewResponse()
        => new();
}

internal sealed class GetCheckoutByIdHandler : IRequestHandler<GetCheckoutByIdRequest, GetCheckoutByIdResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCheckoutByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetCheckoutByIdResponse> Handle(GetCheckoutByIdRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        // work
        var result = await _unitOfWork.CheckoutRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            response.SetFailure($"Checkout with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
            return response;
        }

        // process
        response.SetSuccess(_mapper.Map<CheckoutDto>(result));

        // return
        return response;
    }
}
