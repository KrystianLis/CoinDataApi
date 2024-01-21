using CoinDataApi.Application.Services;

namespace CoinDataApi.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IDataAggregatorService, DataAggregatorService>();
        return services;
    }
}