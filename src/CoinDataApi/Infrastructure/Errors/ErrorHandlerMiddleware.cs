using System.Net;
using System.Text.Json;

namespace CoinDataApi.Infrastructure.Errors;

public class ErrorHandlerMiddleware : IMiddleware
{
    private readonly ILogger<ErrorHandlerMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger, IHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandleErrorAsync(context, ex);
        }
    }

    private async Task HandleErrorAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = _env.IsDevelopment()
            ? new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace!)
            : new ApiException((int)HttpStatusCode.InternalServerError);

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);

    }
}