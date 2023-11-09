using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Clients;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.ClientContacts;

namespace PortalForgeX.Application.Features.ClientContacts;

public record DeleteClientContactRequest(Guid Id) : ICommand<DeleteClientContactResponse>
{
    public DeleteClientContactResponse NewResponse() => new();
}

internal sealed class DeleteClientContactHandler(ILogger<DeleteClientHandler> logger, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteClientContactRequest, DeleteClientContactResponse>
{
    private readonly ILogger<DeleteClientHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<DeleteClientContactResponse> Handle(DeleteClientContactRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        try
        {
            var foundObject = await _unitOfWork.ClientContactRepository.GetByIdAsync(request.Id, cancellationToken);
            if (foundObject is null)
            {
                response.SetFailure($"Client contactperson with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
                return response;
            }

            _ = await _unitOfWork.ClientContactRepository.DeleteAsync(foundObject, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            response.SetSuccess();
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, ex.Message);
            response.SetFailure("There was a technical error.", StatusCodes.Status500InternalServerError);
        }

        return response;
    }
}
