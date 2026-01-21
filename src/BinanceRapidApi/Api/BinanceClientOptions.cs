namespace BinanceRapidApi.Api;

public sealed class BinanceClientOptions
{
    public Uri BaseAddress { get; init; } = new("https://binance43.p.rapidapi.com/");

    /// <summary>RapidAPI key used in the X-RapidAPI-Key header.</summary>
    public required string RapidApiKey { get; init; }

    /// <summary>RapidAPI host used in the X-RapidAPI-Host header.</summary>
    public string RapidApiHost { get; init; } = "binance43.p.rapidapi.com";
}
