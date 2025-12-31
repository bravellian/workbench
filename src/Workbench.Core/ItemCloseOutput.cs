namespace Workbench.Core;

/// <summary>
/// JSON response envelope for closing a work item.
/// </summary>
/// <param name="Ok">True when the item was closed.</param>
/// <param name="Data">Updated item payload and move flag.</param>
public sealed record ItemCloseOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ItemCloseData Data);
