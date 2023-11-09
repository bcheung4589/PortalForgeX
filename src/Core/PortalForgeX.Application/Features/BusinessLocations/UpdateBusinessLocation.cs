using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Shared.Features.BusinessLocations;

namespace PortalForgeX.Application.Features.BusinessLocations;

public record UpdateBusinessLocationRequest(int Id, BusinessLocationDto BusinessLocation) : ICommand<UpdateBusinessLocationResponse>
{
    public UpdateBusinessLocationResponse NewResponse() => new();
}

internal sealed class UpdateBusinessLocationHandler(
    ILogger<UpdateBusinessLocationHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    UpdateBusinessLocationValidation validator)
    : ValidationHandlerBase<UpdateBusinessLocationRequest, UpdateBusinessLocationResponse>(validator)
    , IRequestHandler<UpdateBusinessLocationRequest, UpdateBusinessLocationResponse>
{
    private readonly ILogger<UpdateBusinessLocationHandler> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<UpdateBusinessLocationResponse> Handle(UpdateBusinessLocationRequest request, CancellationToken cancellationToken)
    {
        var response = await ValidateRequestAsync(request, cancellationToken);
        if (response.HasError)
        {
            return response;
        }

        try
        {
            var foundObject = await _unitOfWork.BusinessLocationRepository.GetByIdAsync(request.Id, cancellationToken);
            if (foundObject is null)
            {
                response.SetFailure($"Business location with specified Id ({request.Id}) could not be found.", StatusCodes.Status404NotFound);
                return response;
            }

            _mapper.Map(request.BusinessLocation, foundObject);

            var resultObject = await _unitOfWork.BusinessLocationRepository.UpdateAsync(foundObject, cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (result == 0)
            {
                response.SetFailure("Failed updating business location.");
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

public class UpdateBusinessLocationValidation : AbstractValidator<UpdateBusinessLocationRequest>
{
    public UpdateBusinessLocationValidation()
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
