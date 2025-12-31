namespace Workbench.Core;

/// <summary>
/// JSON response envelope for repository sync output.
/// </summary>
/// <param name="Ok">True when sync completed without errors.</param>
/// <param name="Data">Optional sync results for items, docs, and navigation.</param>
public sealed record RepoSyncOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] RepoSyncData Data);
