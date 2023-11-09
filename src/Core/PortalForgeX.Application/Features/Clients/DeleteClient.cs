using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.Clients;

namespace PortalForgeX.Application.Features.Clients;

public record DeleteClientRequest(Guid Id) : ICommand<DeleteClientResponse>
{
    public DeleteClientResponse NewResponse() => new();
}

internal sealed class DeleteClientHandler(ILogger<DeleteClientHandler> logger, IUnitOfWork unitOfWork) 
    : IRequestHandler<DeleteClientRequest, DeleteClientResponse>
{
    private readonly ILogger<DeleteClientHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<DeleteClientResponse> Handle(DeleteClientRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        try
        {
            var foundObject = await _unitOfWork.ClientRepository.GetByIdAsync(request.Id, cancellationToken);
            if (foundObject is null)
            {
                response.SetFailure($"Client with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
                return response;
            }

            _ = await _unitOfWork.ClientRepository.DeleteAsync(foundObject, cancellationToken);
            _ = await _unitOfWork.SaveChangesAsync(cancellationToken);

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
