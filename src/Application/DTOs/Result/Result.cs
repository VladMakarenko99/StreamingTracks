namespace Application.DTOs.Result;

public class Result<T>
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    
    public T? Body { get; }
    

    private Result(bool isSuccess, string error, T? body)
    {
        IsSuccess = isSuccess;
        Error = error;
        Body = body;
    }

    public static Result<T> Failure(string error) => new Result<T>(false, error, default);
    public static Result<T> Success(T? body = default) => new Result<T>(true, string.Empty, body);
}