using System.Text.Json;
using BinanceRapidApi.Domain;

namespace BinanceRapidApi.Presentation;

public static class ResultWriter
{
    public static async Task WriteAsync(
        IReadOnlyList<TopMoverResult> results,
        string outputDirectory,
        CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(outputDirectory);

        var payload = new
        {
            generatedAtUtc = DateTimeOffset.UtcNow,
            topMovers = results.Select(r => new
            {
                symbol = r.Symbol,
                priceChangePercent24h = r.PriceChangePercent24h,
                avgPrice = r.AvgPrice,
            }),
        };

        var jsonPath = Path.Combine(outputDirectory, "top-movers.json");
        var mdPath = Path.Combine(outputDirectory, "top-movers.md");

        await File.WriteAllTextAsync(
            jsonPath,
            JsonSerializer.Serialize(payload, new JsonSerializerOptions(JsonSerializerDefaults.Web) { WriteIndented = true }),
            cancellationToken);

        await File.WriteAllTextAsync(mdPath, ToMarkdown(results), cancellationToken);
    }

    public static string ToMarkdown(IReadOnlyList<TopMoverResult> results)
    {
        var lines = new List<string>
        {
            "# Top Movers (24h)",
            "",
            "| Symbol | priceChangePercent (24h) | avgPrice |",
            "|---|---:|---:|",
        };

        foreach (var r in results)
        {
            lines.Add($"| {r.Symbol} | {r.PriceChangePercent24h} | {r.AvgPrice} |");
        }

        lines.Add(string.Empty);
        return string.Join(Environment.NewLine, lines);
    }
}
