using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Domain.Entities;
using PortalForgeX.Shared.Features.Clients;

namespace PortalForgeX.Application.Features.Clients;

public record CreateClientRequest(ClientDto Client) : ICommand<CreateClientResponse>
{
    public CreateClientResponse NewResponse() => new();
}

internal sealed class CreateClientHandler(
    ILogger<CreateClientHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    CreateClientValidation validator)
    : ValidationHandlerBase<CreateClientRequest, CreateClientResponse>(validator)
    , IRequestHandler<CreateClientRequest, CreateClientResponse>
{
    private readonly ILogger<CreateClientHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<CreateClientResponse> Handle(CreateClientRequest request, CancellationToken cancellationToken)
    {
        var response = await ValidateRequestAsync(request, cancellationToken);
        if (response.HasError)
        {
            return response;
        }

        try
        {
            var resultObject = await _unitOfWork.ClientRepository.InsertAsync(_mapper.Map<Client>(request.Client), cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (result == 0)
            {
                response.SetFailure($"Failed adding client {request.Client.Name}.");
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

public class CreateClientValidation : AbstractValidator<CreateClientRequest>
{
    public CreateClientValidation()
    {
        RuleFor(x => x.Client.Name)
            .NotEmpty().WithMessage("Name is a required field.")
            .MaximumLength(100).WithMessage("Name can have max 100 chars.");

        RuleFor(x => x.Client.Remarks)
            .MaximumLength(2000).WithMessage("Remarks can have max 2000 chars.");
    }
}
