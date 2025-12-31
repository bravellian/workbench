namespace Workbench.Core;

/// <summary>
/// JSON response envelope for doc sync output.
/// </summary>
/// <param name="Ok">True when sync completed without errors.</param>
/// <param name="Data">Sync counts and missing references.</param>
public sealed record DocSyncOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] DocSyncData Data);
