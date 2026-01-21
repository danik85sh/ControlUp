# Binance RapidAPI Take-Home (C#)

Automation framework in C# for the RapidAPI Binance v4.3 API:
https://rapidapi.com/Glavier/api/binance43

It runs tests that:

1. Identify the 3 symbols with the highest `priceChangePercent` over the last 24 hours (from `/ticker/24hr`).
2. For those symbols, obtain the current average price (from `/avgPrice`).
3. Write results in a consumable format under `artifacts/`.

## Projects

- Library: `src/BinanceRapidApi`
- Tests: `tests/BinanceRapidApi.Tests`

## Prerequisites

- .NET SDK 9.x

## Configuration

Environment variables:

- `RAPIDAPI_KEY` (required to run the integration smoke test)
- `RAPIDAPI_HOST` (optional; defaults to `binance43.p.rapidapi.com`)

PowerShell (current session):

- `$env:RAPIDAPI_KEY = "<your_key>"`
- `$env:RAPIDAPI_HOST = "binance43.p.rapidapi.com"` (optional)

## Run

From the repo root:

- Build: `dotnet build -c Release`
- Tests: `dotnet test -c Release`

Notes:

- Unit tests run without network.
- The integration test is skipped unless `RAPIDAPI_KEY` is set.

## Output

When the integration test runs, it writes:

- `artifacts/top-movers.json`
- `artifacts/top-movers.md`

## CI/CD (GitHub Actions)

Workflow: `.github/workflows/ci.yml`

Secrets to configure in the GitHub repo:

- `RAPIDAPI_KEY`
- `RAPIDAPI_HOST` (optional)

The workflow uploads:

- test results (`*.trx`)
- `artifacts/`

## AI Agents Note (Required)

Tools used:

- GitHub Copilot (GPT-5.2)

Where it helped:

- Drafting and iterating on project structure, typed client/service skeletons, and GitHub Actions workflow.

Where it hindered / what was reviewed:

- Verified endpoint paths, required headers, deterministic sorting, and ensured integration tests are skippable without secrets.
