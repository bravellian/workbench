namespace Workbench.Core;

/// <summary>
/// JSON response envelope for PR creation output.
/// </summary>
/// <param name="Ok">True when PR creation succeeded.</param>
/// <param name="Data">PR URL and item ID.</param>
public sealed record PrOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] PrData Data);
