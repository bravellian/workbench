namespace Workbench.Core;

/// <summary>
/// JSON response envelope for work item status updates.
/// </summary>
/// <param name="Ok">True when the update succeeded.</param>
/// <param name="Data">Updated work item payload.</param>
public sealed record ItemStatusOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ItemStatusData Data);
