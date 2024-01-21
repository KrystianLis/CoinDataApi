using CoinDataApi.Core.Interfaces.Clients;
using CoinDataApi.Infrastructure.Clients;

namespace CoinDataApi.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient<ICoinApiClient, CoinApiClient>(client =>
        {
            client.BaseAddress = new Uri(configuration.GetValue<string>("CoinApi:Url")!);
        });

        services.Configure<CoinApiOptions>(configuration.GetRequiredSection("CoinApi"));

        return services;
    }
}