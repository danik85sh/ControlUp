using BinanceRapidApi.Domain;

namespace BinanceRapidApi.Api;

public interface IBinanceClient
{
    Task<IReadOnlyList<Ticker24h>> GetTicker24hAsync(CancellationToken cancellationToken);
    Task<AvgPrice> GetAvgPriceAsync(string symbol, CancellationToken cancellationToken);
}
