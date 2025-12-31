namespace Workbench.Core;

/// <summary>
/// JSON response envelope for promote output.
/// </summary>
/// <param name="Ok">True when promotion succeeded.</param>
/// <param name="Data">Promotion results.</param>
public sealed record PromoteOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] PromoteData Data);
