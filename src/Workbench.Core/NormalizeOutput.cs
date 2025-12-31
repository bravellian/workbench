namespace Workbench.Core;

/// <summary>
/// JSON response envelope for normalize output.
/// </summary>
/// <param name="Ok">True when normalization succeeded.</param>
/// <param name="Data">Normalization results.</param>
public sealed record NormalizeOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] NormalizeData Data);
