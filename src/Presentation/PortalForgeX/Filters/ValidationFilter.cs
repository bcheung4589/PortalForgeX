using FluentValidation;

namespace PortalForgeX.Filters;

public class ValidationFilter<TModel>(IValidator<TModel> validator) : IEndpointFilter
    where TModel : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validationResult = await validator.ValidateAsync(context.GetArgument<TModel>(0));
        if (validationResult is not null && !validationResult.IsValid)
        {
            return Results.Problem("Validation failed.");
        }

        return await next(context);
    }
}
