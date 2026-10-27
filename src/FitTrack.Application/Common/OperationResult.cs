namespace FitTrack.Application.Common;

public sealed record OperationResult<T>(bool IsSuccess, T? Value, string? Error, string? ErrorCode)
{
    public static OperationResult<T> Success(T value) => new(true, value, null, null);
    public static OperationResult<T> Failure(string error, string code = "operation_failed") => new(false, default, error, code);
}
