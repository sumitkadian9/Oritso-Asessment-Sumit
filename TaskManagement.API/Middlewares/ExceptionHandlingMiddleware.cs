using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace TaskManagement.API.Middlewares;

public sealed class ExceptionHandlingMiddleware(IHostEnvironment env, ILogger<ExceptionHandlingMiddleware> logger) : IMiddleware
{
    private readonly IHostEnvironment _env = env;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "InternalServerError", 
            Detail = _env.IsDevelopment() ? ex.Message : "An unexpected error occurred on the server.",
            Instance = context.Request.Path
        };

        if (_env.IsDevelopment())
        {
            problemDetails.Extensions["stackTrace"] = ex.StackTrace;
            if (ex.InnerException != null)
            {
                problemDetails.Extensions["innerException"] = ex.InnerException.Message;
            }
        }

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(problemDetails, options);

        await context.Response.WriteAsync(json);
    }
}
