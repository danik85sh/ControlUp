using BinanceRapidApi.Api;
using BinanceRapidApi.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BinanceRapidApi.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBinanceRapidApi(this IServiceCollection services, BinanceClientOptions options)
    {
        services.AddSingleton(options);

        services.AddHttpClient<IBinanceClient, BinanceClient>(client =>
        {
            client.BaseAddress = options.BaseAddress;
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        services.AddSingleton<MarketMoversService>();

        return services;
    }
}
