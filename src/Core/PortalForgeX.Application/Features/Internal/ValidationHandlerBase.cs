using FluentValidation;
using Microsoft.AspNetCore.Http;
using PortalForgeX.Shared;

namespace PortalForgeX.Application.Features.Internal;

internal abstract class ValidationHandlerBase<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
    where TResponse : Result
{
    private readonly IValidator<TRequest> _validator;

    protected ValidationHandlerBase(IValidator<TRequest> validator)
        => _validator = validator;

    /// <summary>
    /// Validates the request and call the failure on the response if invalid.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected async Task<TResponse> ValidateRequestAsync(TRequest request, CancellationToken cancellationToken)
    {
        var response = request.NewResponse();

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid)
        {
            return response;
        }

        response.SetFailure(validationResult.Errors.Select(x => x.ErrorMessage), StatusCodes.Status400BadRequest);

        return response;
    }
}
