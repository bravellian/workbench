namespace Workbench.Core;

/// <summary>
/// Payload describing a config update operation.
/// </summary>
/// <param name="Path">Path to the config file.</param>
/// <param name="Config">Updated configuration values.</param>
/// <param name="Changed">True when the config content changed.</param>
public sealed record ConfigSetData(
    [property: JsonPropertyName("path")] string Path,
    [property: JsonPropertyName("config")] WorkbenchConfig Config,
    [property: JsonPropertyName("changed")] bool Changed);
