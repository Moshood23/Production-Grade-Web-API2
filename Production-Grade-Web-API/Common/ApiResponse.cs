namespace Production.Grade.WebApi.API.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new();
    public int StatusCode { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static ApiResponse<T> SuccessResponse(T data, int statusCode = 200)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            StatusCode = statusCode,
            Errors = new()
        };
    }

    public static ApiResponse<T> FailureResponse(List<string> errors, int statusCode = 400)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Data = default,
            StatusCode = statusCode,
            Errors = errors
        };
    }

    public static ApiResponse<T> FailureResponse(string error, int statusCode = 400)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Data = default,
            StatusCode = statusCode,
            Errors = new List<string> { error }
        };
    }
}

public class ApiResponse
{
    public bool Success { get; set; }
    public List<string> Errors { get; set; } = new();
    public int StatusCode { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static ApiResponse SuccessResponse(int statusCode = 200)
    {
        return new ApiResponse
        {
            Success = true,
            StatusCode = statusCode,
            Errors = new()
        };
    }

    public static ApiResponse FailureResponse(List<string> errors, int statusCode = 400)
    {
        return new ApiResponse
        {
            Success = false,
            StatusCode = statusCode,
            Errors = errors
        };
    }

    public static ApiResponse FailureResponse(string error, int statusCode = 400)
    {
        return new ApiResponse
        {
            Success = false,
            StatusCode = statusCode,
            Errors = new List<string> { error }
        };
    }
}
