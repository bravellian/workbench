namespace Workbench.Core;

/// <summary>
/// JSON response envelope for config show output.
/// </summary>
/// <param name="Ok">True when config loaded without errors.</param>
/// <param name="Data">Resolved config and source metadata.</param>
public sealed record ConfigOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ConfigData Data);
