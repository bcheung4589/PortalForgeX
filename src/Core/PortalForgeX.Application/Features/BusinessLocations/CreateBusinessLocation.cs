using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Domain.Entities;
using PortalForgeX.Shared.Features.BusinessLocations;

namespace PortalForgeX.Application.Features.BusinessLocations;

public record CreateBusinessLocationRequest(BusinessLocationDto BusinessLocation) : ICommand<CreateBusinessLocationResponse>
{
    public CreateBusinessLocationResponse NewResponse() => new();
}

internal sealed class CreateBusinessLocationHandler(
    ILogger<CreateBusinessLocationHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    CreateBusinessLocationValidation validator)
    : ValidationHandlerBase<CreateBusinessLocationRequest, CreateBusinessLocationResponse>(validator)
    , IRequestHandler<CreateBusinessLocationRequest, CreateBusinessLocationResponse>
{
    private readonly ILogger<CreateBusinessLocationHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<CreateBusinessLocationResponse> Handle(CreateBusinessLocationRequest request, CancellationToken cancellationToken)
    {
        var response = await ValidateRequestAsync(request, cancellationToken);
        if (response.HasError)
        {
            return response;
        }

        try
        {
            var resultObject = await _unitOfWork.BusinessLocationRepository.InsertAsync(_mapper.Map<BusinessLocation>(request.BusinessLocation), cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (result == 0)
            {
                response.SetFailure("Failed adding business location.");
                return response;
            }

            response.SetSuccess(_mapper.Map<BusinessLocationDto>(resultObject));
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, ex.Message);
            response.SetFailure("There was a technical error.", StatusCodes.Status500InternalServerError);
        }

        return response;
    }
}

public class CreateBusinessLocationValidation : AbstractValidator<CreateBusinessLocationRequest>
{
    public CreateBusinessLocationValidation()
    {
        RuleFor(x => x.BusinessLocation.ApiKey)
            .NotEmpty().WithMessage("ApiKey is a required field.")
            .MaximumLength(100).WithMessage("ApiKey can have max 100 chars.");

        RuleFor(x => x.BusinessLocation.Street)
            .NotEmpty().WithMessage("Street is a required field.")
            .MaximumLength(200).WithMessage("Street can have max 200 chars.");

        RuleFor(x => x.BusinessLocation.HouseNr)
            .NotEmpty().WithMessage("House Nr is a required field.")
            .MaximumLength(10).WithMessage("House Nr can have max 10 chars.");

        RuleFor(x => x.BusinessLocation.Zipcode)
            .NotEmpty().WithMessage("Zipcode is a required field.")
            .MaximumLength(10).WithMessage("Zipcode can have max 10 chars.");

        RuleFor(x => x.BusinessLocation.Place)
            .NotEmpty().WithMessage("Place is a required field.")
            .MaximumLength(100).WithMessage("Place can have max 100 chars.");

        RuleFor(x => x.BusinessLocation.Country)
            .NotEmpty().WithMessage("Country is a required field.")
            .MaximumLength(100).WithMessage("Country can have max 100 chars.");

        RuleFor(x => x.BusinessLocation.IpAddress)
            .MaximumLength(10).WithMessage("IpAddress can have max 10 chars.");

        RuleFor(x => x.BusinessLocation.Remarks)
            .MaximumLength(2000).WithMessage("Remarks can have max 2000 chars.");
    }
}
