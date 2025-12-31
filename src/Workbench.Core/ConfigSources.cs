namespace Workbench.Core;

/// <summary>
/// Metadata describing where config values were sourced.
/// </summary>
/// <param name="Defaults">True when defaults are included.</param>
/// <param name="RepoConfig">Path to the repo config file.</param>
public sealed record ConfigSources(
    [property: JsonPropertyName("defaults")] bool Defaults,
    [property: JsonPropertyName("repoConfig")] string RepoConfig);
