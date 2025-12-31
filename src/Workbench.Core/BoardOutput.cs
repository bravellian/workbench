namespace Workbench.Core;

/// <summary>
/// JSON response envelope for workboard regeneration output.
/// </summary>
/// <param name="Ok">True when regeneration succeeded.</param>
/// <param name="Data">Workboard path and counts.</param>
public sealed record BoardOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] BoardData Data);
