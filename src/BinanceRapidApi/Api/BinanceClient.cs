using System.Net.Http.Json;
using System.Text.Json;
using BinanceRapidApi.Domain;

namespace BinanceRapidApi.Api;

public sealed class BinanceClient(HttpClient httpClient, BinanceClientOptions options) : IBinanceClient
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true,
    };

    public async Task<IReadOnlyList<Ticker24h>> GetTicker24hAsync(CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "ticker/24hr");
        AddRapidApiHeaders(request);

        using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var snippet = await ReadSnippetAsync(response, cancellationToken);
            throw new BinanceApiException($"/ticker/24hr call failed ({(int)response.StatusCode}).", (int)response.StatusCode, snippet);
        }

        var payload = await response.Content.ReadFromJsonAsync<List<Ticker24h>>(SerializerOptions, cancellationToken);
        return payload ?? [];
    }

    public async Task<AvgPrice> GetAvgPriceAsync(string symbol, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(symbol))
        {
            throw new ArgumentException("Symbol is required.", nameof(symbol));
        }

        var url = $"avgPrice?symbol={Uri.EscapeDataString(symbol)}";
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        AddRapidApiHeaders(request);

        using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var snippet = await ReadSnippetAsync(response, cancellationToken);
            throw new BinanceApiException($"/avgPrice call failed for '{symbol}' ({(int)response.StatusCode}).", (int)response.StatusCode, snippet);
        }

        var payload = await response.Content.ReadFromJsonAsync<AvgPrice>(SerializerOptions, cancellationToken);
        if (payload is null)
        {
            throw new BinanceApiException($"/avgPrice returned empty payload for '{symbol}'.");
        }

        return payload;
    }

    private void AddRapidApiHeaders(HttpRequestMessage request)
    {
        request.Headers.TryAddWithoutValidation("X-RapidAPI-Key", options.RapidApiKey);
        request.Headers.TryAddWithoutValidation("X-RapidAPI-Host", options.RapidApiHost);
    }

    private static async Task<string?> ReadSnippetAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        try
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            if (string.IsNullOrWhiteSpace(body))
            {
                return null;
            }

            return body.Length <= 500 ? body : body[..500];
        }
        catch
        {
            return null;
        }
    }
}
