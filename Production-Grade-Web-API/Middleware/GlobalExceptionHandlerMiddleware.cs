namespace Production.Grade.WebApi.API.Middleware;

using Production.Grade.WebApi.API.Common;
using System.Net;
using System.Text.Json;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError($"Unhandled exception: {exception.Message}");
            await HandleExceptionAsync(context, exception);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ApiResponse();

        switch (exception)
        {
            case KeyNotFoundException:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                response = ApiResponse.FailureResponse(exception.Message, StatusCodes.Status404NotFound);
                break;

            case InvalidOperationException:
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                response = ApiResponse.FailureResponse(exception.Message, StatusCodes.Status409Conflict);
                break;

            case UnauthorizedAccessException:
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                response = ApiResponse.FailureResponse(exception.Message, StatusCodes.Status401Unauthorized);
                break;

            case ArgumentException:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response = ApiResponse.FailureResponse(exception.Message, StatusCodes.Status400BadRequest);
                break;

            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response = ApiResponse.FailureResponse("An unexpected error occurred", StatusCodes.Status500InternalServerError);
                break;
        }

        return context.Response.WriteAsJsonAsync(response);
    }
}
