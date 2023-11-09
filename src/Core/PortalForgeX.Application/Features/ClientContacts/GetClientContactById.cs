using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.ClientContacts;

namespace PortalForgeX.Application.Features.ClientContacts;

public record GetClientContactByIdRequest(Guid Id) : ICommand<GetClientContactByIdResponse>
{
    public GetClientContactByIdResponse NewResponse() => new();
}

internal sealed class GetClientContactByIdHandler(ILogger<GetClientContactByIdHandler> logger, IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetClientContactByIdRequest, GetClientContactByIdResponse>
{
    private readonly ILogger<GetClientContactByIdHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<GetClientContactByIdResponse> Handle(GetClientContactByIdRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        try
        {
            var result = await _unitOfWork.ClientContactRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                response.SetFailure($"Client contactperson with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
                return response;
            }

            response.SetSuccess(_mapper.Map<ClientContactDto>(result));
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, ex.Message);
            response.SetFailure("There was a technical error.", StatusCodes.Status500InternalServerError);
        }

        return response;
    }
}
