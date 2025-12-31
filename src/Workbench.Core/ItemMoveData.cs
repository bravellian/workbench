namespace Workbench.Core;

/// <summary>
/// Payload for work item move operations.
/// </summary>
/// <param name="Item">Updated work item payload.</param>
public sealed record ItemMoveData(
    [property: JsonPropertyName("item")] WorkItemPayload Item);
