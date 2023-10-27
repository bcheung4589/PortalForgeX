using PortalForgeX.Shared;

namespace PortalForgeX.Extensions;

public static class ResultExtensions
{
    /// <summary>
    /// Handle any errors in the result and return the appropiate response.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static IResult ToResponse(this Result result)
    {
        result.ServerExecutionDuration = DateTime.UtcNow - result.ServerExecutionStartTime;

        if (result.HasError)
        {
            IResult? response = null;

            // handle error code
            if (result.ErrorCode is not null)
            {
                response = result.ErrorCode switch
                {
                    StatusCodes.Status400BadRequest => Results.BadRequest(result.ErrorMessages),
                    StatusCodes.Status401Unauthorized => Results.Unauthorized(),
                    StatusCodes.Status404NotFound => Results.NotFound(result.ErrorMessages),
                    StatusCodes.Status409Conflict => Results.Conflict(result.ErrorMessages),
                    _ => Results.StatusCode(result.ErrorCode.Value)
                };
            }

            // general error
            response ??= Results.BadRequest(result.ErrorMessages);

            return response;
        }

        // success
        return Results.Ok(result);
    }
}
