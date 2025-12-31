namespace Workbench.Core;

/// <summary>
/// Payload for work item close operations.
/// </summary>
/// <param name="Item">Updated work item payload.</param>
/// <param name="Moved">True when the item file was moved to done.</param>
public sealed record ItemCloseData(
    [property: JsonPropertyName("item")] WorkItemPayload Item,
    [property: JsonPropertyName("moved")] bool Moved);
