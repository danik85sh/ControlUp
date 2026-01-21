# QA Automation Engineer Take-Home — Notes

## What this repo does

- Calls `/ticker/24hr` and selects the 3 symbols with the highest `priceChangePercent` over the last 24 hours.
- Calls `/avgPrice` for those 3 symbols.
- Writes results into `artifacts/` as JSON + Markdown.

## Structure

- Library: `src/BinanceRapidApi`
- Tests: `tests/BinanceRapidApi.Tests`

## Configuration

- `RAPIDAPI_KEY` (required for integration test)
- `RAPIDAPI_HOST` (optional; defaults to `binance43.p.rapidapi.com`)

## Testing approach

- Unit tests validate parsing/sorting deterministically (no network).
- Integration test is skipped unless `RAPIDAPI_KEY` is present.

## CI

GitHub Actions workflow builds and runs tests and uploads `artifacts/`.
