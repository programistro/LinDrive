namespace LinDrive.Shared;

public class Result<T>
{
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;
    
    public T? Value { get; set; }
    
    public string? Error { get; set; }

    private Result(T? Value, bool isSuccess, string? error = null)
    {
        this.Value = Value;
        this.IsSuccess = isSuccess;
        this.Error = error;
    }
    
    public static Result<T> Success(T value) => new Result<T>(value, true, null);
    
    public static Result<T> Failure(string error) => new Result<T>(default, false,  error);
}