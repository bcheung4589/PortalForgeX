using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Domain.Entities;
using PortalForgeX.Shared.Features.ClientContacts;

namespace PortalForgeX.Application.Features.ClientContacts;

public record CreateClientContactRequest(ClientContactDto ClientContact) : ICommand<CreateClientContactResponse>
{
    public CreateClientContactResponse NewResponse() => new();
}

internal sealed class CreateClientContactHandler(
    ILogger<CreateClientContactHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    CreateClientContactValidation validator)
    : ValidationHandlerBase<CreateClientContactRequest, CreateClientContactResponse>(validator)
    , IRequestHandler<CreateClientContactRequest, CreateClientContactResponse>
{
    private readonly ILogger<CreateClientContactHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<CreateClientContactResponse> Handle(CreateClientContactRequest request, CancellationToken cancellationToken)
    {
        var response = await ValidateRequestAsync(request, cancellationToken);
        if (response.HasError)
        {
            return response;
        }

        try
        {
            var resultObject = await _unitOfWork.ClientContactRepository.InsertAsync(_mapper.Map<ClientContact>(request.ClientContact), cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (result == 0)
            {
                response.SetFailure("Failed adding client contactperson.");
                return response;
            }

            response.SetSuccess(_mapper.Map<ClientContactDto>(resultObject));
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, ex.Message);
            response.SetFailure("There was a technical error.", StatusCodes.Status500InternalServerError);
        }

        return response;
    }
}

public class CreateClientContactValidation : AbstractValidator<CreateClientContactRequest>
{
    public CreateClientContactValidation()
    {
        RuleFor(x => x.ClientContact.FullName)
            .NotEmpty().WithMessage("Name is a required field.")
            .MaximumLength(100).WithMessage("Name can have max 100 chars.");

        RuleFor(x => x.ClientContact.PhoneNr)
            .NotEmpty().WithMessage("Phone Nr is a required field.")
            .MaximumLength(20).WithMessage("Phone Nr can have max 20 chars.");

        RuleFor(x => x.ClientContact.Email)
            .MaximumLength(150).WithMessage("Email can have max 150 chars.");

        RuleFor(x => x.ClientContact.Remarks)
            .MaximumLength(2000).WithMessage("Remarks can have max 2000 chars.");
    }
}
