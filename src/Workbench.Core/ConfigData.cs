namespace Workbench.Core;

/// <summary>
/// Payload describing the effective config and its sources.
/// </summary>
/// <param name="Config">Resolved configuration values.</param>
/// <param name="Sources">Metadata about config origins.</param>
public sealed record ConfigData(
    [property: JsonPropertyName("config")] WorkbenchConfig Config,
    [property: JsonPropertyName("sources")] ConfigSources Sources);
