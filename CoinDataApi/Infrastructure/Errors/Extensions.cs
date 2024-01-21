namespace CoinDataApi.Infrastructure.Errors;

public static class Extensions
{
    public static IServiceCollection AddErrorHandler(this IServiceCollection services)
        => services.AddScoped<ErrorHandlerMiddleware>();

    public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder app)
        => app.UseMiddleware<ErrorHandlerMiddleware>();
}