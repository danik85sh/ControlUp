using System.Globalization;
using BinanceRapidApi.Api;
using BinanceRapidApi.Domain;

namespace BinanceRapidApi.Services;

public sealed class MarketMoversService(IBinanceClient client)
{
    public async Task<IReadOnlyList<TopMoverResult>> GetTopMoversWithAvgPriceAsync(int top, CancellationToken cancellationToken)
    {
        if (top <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(top), top, "Top must be positive.");
        }

        var tickers = await client.GetTicker24hAsync(cancellationToken);
        var topMovers = SelectTopMovers(tickers, top);

        var tasks = topMovers
            .Select(async item =>
            {
                var avg = await client.GetAvgPriceAsync(item.Symbol, cancellationToken);
                if (!TryParseDecimal(avg.Price, out var avgValue))
                {
                    throw new BinanceApiException($"avgPrice.price was not a valid decimal for '{item.Symbol}'.");
                }

                return new TopMoverResult(item.Symbol, item.PriceChangePercent24h, avgValue);
            })
            .ToArray();

        return await Task.WhenAll(tasks);
    }

    public static IReadOnlyList<(string Symbol, decimal PriceChangePercent24h)> SelectTopMovers(IEnumerable<Ticker24h> tickers, int top)
    {
        return tickers
            .Select(t =>
            {
                if (string.IsNullOrWhiteSpace(t.Symbol))
                {
                    return (Ok: false, Symbol: string.Empty, Percent: 0m);
                }

                if (!TryParseDecimal(t.PriceChangePercent, out var pct))
                {
                    return (Ok: false, Symbol: string.Empty, Percent: 0m);
                }

                return (Ok: true, Symbol: t.Symbol!, Percent: pct);
            })
            .Where(x => x.Ok)
            .Select(x => (x.Symbol, PriceChangePercent24h: x.Percent))
            .OrderByDescending(x => x.PriceChangePercent24h)
            .ThenBy(x => x.Symbol, StringComparer.Ordinal)
            .Take(top)
            .ToArray();
    }

    private static bool TryParseDecimal(string? value, out decimal result)
        => decimal.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
}
