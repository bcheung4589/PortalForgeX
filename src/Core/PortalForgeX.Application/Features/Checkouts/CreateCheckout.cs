using AutoMapper;
using MediatR;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Domain.Entities;
using PortalForgeX.Shared.Features.Checkouts;

namespace PortalForgeX.Application.Features.Checkouts;

public record CreateCheckoutRequest(CheckoutDto Checkout) : ICommand<CreateCheckoutResponse>
{
    public CreateCheckoutResponse NewResponse()
        => new();
}

internal sealed class CreateCheckoutHandler : IRequestHandler<CreateCheckoutRequest, CreateCheckoutResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCheckoutHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateCheckoutResponse> Handle(CreateCheckoutRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        // work
        var resultObject = await _unitOfWork.CheckoutRepository.InsertAsync(_mapper.Map<Checkout>(request.Checkout), cancellationToken);
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (result == 0)
        {
            response.SetFailure("Failed adding checkout.");
            return response;
        }

        // process
        response.SetSuccess(_mapper.Map<CheckoutDto>(resultObject));

        // return
        return response;
    }
}
