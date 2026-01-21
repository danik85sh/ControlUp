using BinanceRapidApi.Api;
using BinanceRapidApi.DependencyInjection;
using BinanceRapidApi.Presentation;
using BinanceRapidApi.Services;
using BinanceRapidApi.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BinanceRapidApi.Tests.Integration;

public sealed class RapidApiSmokeTests
{
    private const string RapidApiKeyEnvVar = "RAPIDAPI_KEY";
    private const string RapidApiHostEnvVar = "RAPIDAPI_HOST";

    [RequiresRapidApiKeyFact]
    public async Task Top3Movers_CanBeFetched_AndWrittenToRepoArtifacts()
    {
        var apiKey = Environment.GetEnvironmentVariable(RapidApiKeyEnvVar);
        Assert.False(string.IsNullOrWhiteSpace(apiKey));

        var host = Environment.GetEnvironmentVariable(RapidApiHostEnvVar);

        var services = new ServiceCollection();
        services.AddBinanceRapidApi(new BinanceClientOptions
        {
            RapidApiKey = apiKey,
            RapidApiHost = string.IsNullOrWhiteSpace(host) ? "binance43.p.rapidapi.com" : host,
            BaseAddress = new Uri("https://binance43.p.rapidapi.com/")
        });

        var provider = services.BuildServiceProvider();
        var svc = provider.GetRequiredService<MarketMoversService>();

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        var results = await svc.GetTopMoversWithAvgPriceAsync(3, cts.Token);

        Assert.Equal(3, results.Count);
        Assert.All(results, r => Assert.False(string.IsNullOrWhiteSpace(r.Symbol)));

        var symbols = results.Select(r => r.Symbol).ToArray();
        Assert.Equal(symbols.Length, symbols.Distinct(StringComparer.Ordinal).Count());

        var repoRoot = RepoPaths.FindRepoRoot();
        var artifactsDir = Path.Combine(repoRoot, "artifacts");
        await ResultWriter.WriteAsync(results, artifactsDir, cts.Token);
    }
}
