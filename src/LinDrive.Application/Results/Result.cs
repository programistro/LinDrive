namespace LinDrive.Application.Results;

public class Result<T>
{
    public bool IsSuccess { get; }
    
    public bool IsFailure =>  !this.IsSuccess;
    
    public T? Value { get; }
    
    public string? Error { get; set; }

    private Result(T? value, bool isSuccess, string? error = null)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }
    
    public static Result<T> Success(T value) => new Result<T>(value, true, null);
    
    public static Result<T> Failure(string error) => new Result<T>(default, false, error);
}