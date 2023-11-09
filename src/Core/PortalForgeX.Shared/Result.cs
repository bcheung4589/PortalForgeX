namespace PortalForgeX.Shared;

/// <summary>
/// Simple Result record for containing information about a request and containing possible Data.
/// </summary>
/// <typeparam name="T"></typeparam>
public record Result<T> : Result
{
    public T? Data { get; set; }

    public void SetSuccess(T? data)
    {
        IsSuccess = true;
        Data = data;
    }
}

/// <summary>
/// Simple Result record for containing information about a request.
/// </summary>
public record Result
{
    public bool IsSuccess { get; set; }

    public bool HasError { get; set; }

    public IEnumerable<string>? ErrorMessages { get; set; }

    public int? ErrorCode { get; set; }

    public DateTime ServerExecutionStartTime { get; set; }

    public TimeSpan ServerExecutionDuration { get; set; }

    public Result()
    {
        ServerExecutionStartTime = DateTime.UtcNow;
    }

    public void SetSuccess()
    {
        IsSuccess = true;
    }

    public void SetFailure(IEnumerable<string> errors, int? errorCode = null)
    {
        IsSuccess = false;
        HasError = true;
        ErrorMessages = errors;
        ErrorCode = errorCode;
    }

    public void SetFailure(string errorMessage, int? errorCode = null)
    {
        IsSuccess = false;
        HasError = true;
        ErrorMessages = new string[] { errorMessage };
        ErrorCode = errorCode;
    }
}
