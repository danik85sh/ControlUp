namespace BinanceRapidApi.Domain;

public sealed record TopMoverResult(
    string Symbol,
    decimal PriceChangePercent24h,
    decimal AvgPrice
);
