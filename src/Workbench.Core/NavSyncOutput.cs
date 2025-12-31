namespace Workbench.Core;

/// <summary>
/// JSON response envelope for navigation sync output.
/// </summary>
/// <param name="Ok">True when navigation sync succeeded.</param>
/// <param name="Data">Navigation sync results.</param>
public sealed record NavSyncOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] NavSyncData Data);
