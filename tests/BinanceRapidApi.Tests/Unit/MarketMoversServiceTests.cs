using BinanceRapidApi.Domain;
using BinanceRapidApi.Services;

namespace BinanceRapidApi.Tests.Unit;

public sealed class MarketMoversServiceTests
{
    [Fact]
    public void SelectTopMovers_SortsDescending_AndIsDeterministic()
    {
        var tickers = new[]
        {
            new Ticker24h { Symbol = "AAA", PriceChangePercent = "5.0" },
            new Ticker24h { Symbol = "BBB", PriceChangePercent = "5.0" },
            new Ticker24h { Symbol = "CCC", PriceChangePercent = "7.25" },
            new Ticker24h { Symbol = "DDD", PriceChangePercent = "-1" },
        };

        var top = MarketMoversService.SelectTopMovers(tickers, 3);

        Assert.Equal(3, top.Count);
        Assert.Equal(("CCC", 7.25m), top[0]);
        Assert.Equal(("AAA", 5.0m), top[1]);
        Assert.Equal(("BBB", 5.0m), top[2]);
    }

    [Fact]
    public void SelectTopMovers_IgnoresInvalidRows()
    {
        var tickers = new[]
        {
            new Ticker24h { Symbol = null, PriceChangePercent = "1" },
            new Ticker24h { Symbol = "AAA", PriceChangePercent = null },
            new Ticker24h { Symbol = "BBB", PriceChangePercent = "not-a-number" },
            new Ticker24h { Symbol = "CCC", PriceChangePercent = "2.5" },
            new Ticker24h { Symbol = "DDD", PriceChangePercent = "1.0" },
        };

        var top = MarketMoversService.SelectTopMovers(tickers, 3);

        Assert.Equal(2, top.Count);
        Assert.Equal(("CCC", 2.5m), top[0]);
        Assert.Equal(("DDD", 1.0m), top[1]);
    }
}
