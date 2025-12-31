namespace Workbench.Core;

/// <summary>
/// JSON response envelope for item normalization output.
/// </summary>
/// <param name="Ok">True when normalization succeeded.</param>
/// <param name="Data">Normalization counts and dry-run flag.</param>
public sealed record ItemNormalizeOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ItemNormalizeData Data);
