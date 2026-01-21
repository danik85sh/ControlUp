using System.Text.Json.Serialization;

namespace BinanceRapidApi.Domain;

public sealed record Ticker24h
{
    [JsonPropertyName("symbol")]
    public string? Symbol { get; init; }

    [JsonPropertyName("priceChangePercent")]
    public string? PriceChangePercent { get; init; }
}
