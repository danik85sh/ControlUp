using System.Text.Json.Serialization;

namespace BinanceRapidApi.Domain;

public sealed record AvgPrice
{
    [JsonPropertyName("mins")]
    public int? Mins { get; init; }

    [JsonPropertyName("price")]
    public string? Price { get; init; }
}
