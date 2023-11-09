using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.Clients;

namespace PortalForgeX.Application.Features.Clients;

public record UpdateClientRequest(Guid Id, ClientDto Client) : ICommand<UpdateClientResponse>
{
    public UpdateClientResponse NewResponse() => new();
}

internal sealed class UpdateClientHandler(
    ILogger<UpdateClientHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    UpdateClientValidation validator)
    : ValidationHandlerBase<UpdateClientRequest, UpdateClientResponse>(validator)
    , IRequestHandler<UpdateClientRequest, UpdateClientResponse>
{
    private readonly ILogger<UpdateClientHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<UpdateClientResponse> Handle(UpdateClientRequest request, CancellationToken cancellationToken)
    {
        var response = await ValidateRequestAsync(request, cancellationToken);
        if (response.HasError)
        {
            return response;
        }

        try
        {
            var foundObject = await _unitOfWork.ClientRepository.GetByIdAsync(request.Id, cancellationToken);
            if (foundObject is null)
            {
                response.SetFailure($"Client with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
                return response;
            }

            _mapper.Map(request.Client, foundObject);

            var resultObject = await _unitOfWork.ClientRepository.UpdateAsync(foundObject, cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (result == 0)
            {
                response.SetFailure("Failed updating client.");
                return response;
            }

            response.SetSuccess(_mapper.Map<ClientDto>(resultObject));
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, ex.Message);
            response.SetFailure("There was a technical error.", StatusCodes.Status500InternalServerError);
        }

        return response;
    }
}

public class UpdateClientValidation : AbstractValidator<UpdateClientRequest>
{
    public UpdateClientValidation()
    {
        RuleFor(x => x.Client.Name)
            .NotEmpty().WithMessage("Name is a required field.")
            .MaximumLength(100).WithMessage("Name can have max 100 chars.");

        RuleFor(x => x.Client.Remarks)
            .MaximumLength(2000).WithMessage("Remarks can have max 2000 chars.");
    }
}
