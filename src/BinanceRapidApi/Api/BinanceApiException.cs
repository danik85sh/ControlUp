namespace BinanceRapidApi.Api;

public sealed class BinanceApiException : Exception
{
    public BinanceApiException(string message, int? statusCode = null, string? responseBodySnippet = null, Exception? innerException = null)
        : base(message, innerException)
    {
        StatusCode = statusCode;
        ResponseBodySnippet = responseBodySnippet;
    }

    public int? StatusCode { get; }
    public string? ResponseBodySnippet { get; }
}
