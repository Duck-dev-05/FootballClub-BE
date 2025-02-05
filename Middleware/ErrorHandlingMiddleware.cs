using System.Net;
using System.Text.Json;
using FootballClub_Backend.Exceptions;

namespace FootballClub_Backend.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public ErrorHandlingMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlingMiddleware> logger,
        IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var errorResponse = new ErrorResponse
            {
                Type = error.GetType().Name,
                Message = error.Message,
                StackTrace = _env.IsDevelopment() ? error.StackTrace : null
            };

            switch (error)
            {
                case AuthenticationException:
                case UnauthorizedAccessException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case KeyNotFoundException:
                case FileNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case ArgumentException:
                case ValidationException:
                case FormatException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case InvalidOperationException:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    break;
                default:
                    _logger.LogError(error, "An unhandled exception occurred");
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = "An internal server error occurred.";
                    break;
            }

            var result = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            });
            
            await response.WriteAsync(result);
        }
    }
}

public class ErrorResponse
{
    public string Type { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? StackTrace { get; set; }
} 