# Workbench

Workbench is a .NET-based CLI for interacting with the Workbench tooling.

## Quickstart

### Prerequisites

- .NET SDK (latest stable recommended)

### Build

```bash
dotnet build Workbench.slnx
```

### Run (CLI)

```bash
dotnet run --project src/Workbench/Workbench.csproj -- --help
```

### Test

```bash
dotnet test tests/Workbench.Tests/Workbench.Tests.csproj
```

## Verification

Run the full test suite (matches CI expectations):

```bash
dotnet test Workbench.slnx
```

## Build (AOT)

Publish a single native binary:

```bash
dotnet publish src/Workbench/Workbench.csproj -c Release -r osx-arm64
```

Replace the runtime identifier with your target (e.g., `win-x64`, `linux-x64`).

## Command reference

See the full CLI command list and options in `docs/30-contracts/cli-help.md`.

## Contributing

- [Contribution guide](CONTRIBUTING.md)
- [Code of Conduct](CODE_OF_CONDUCT.md)
- [Security policy](SECURITY.md)
