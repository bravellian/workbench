namespace Workbench.Core;

/// <summary>
/// Payload for work item status updates.
/// </summary>
/// <param name="Item">Updated work item payload.</param>
public sealed record ItemStatusData(
    [property: JsonPropertyName("item")] WorkItemPayload Item);
