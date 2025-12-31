namespace Workbench.Core;

/// <summary>
/// JSON response envelope for work item sync output.
/// </summary>
/// <param name="Ok">True when sync completed without errors.</param>
/// <param name="Data">Sync results and warnings.</param>
public sealed record ItemSyncOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ItemSyncData Data);
