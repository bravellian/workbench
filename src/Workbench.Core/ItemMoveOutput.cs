namespace Workbench.Core;

/// <summary>
/// JSON response envelope for work item move operations.
/// </summary>
/// <param name="Ok">True when the move succeeded.</param>
/// <param name="Data">Updated work item payload.</param>
public sealed record ItemMoveOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ItemMoveData Data);
