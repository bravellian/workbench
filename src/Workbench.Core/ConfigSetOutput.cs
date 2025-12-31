namespace Workbench.Core;

/// <summary>
/// JSON response envelope for config set output.
/// </summary>
/// <param name="Ok">True when the update succeeded.</param>
/// <param name="Data">Details about the update.</param>
public sealed record ConfigSetOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ConfigSetData Data);
