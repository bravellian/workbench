namespace Workbench.Core;

/// <summary>
/// Payload for work item rename operations.
/// </summary>
/// <param name="Item">Updated work item payload.</param>
public sealed record ItemRenameData(
    [property: JsonPropertyName("item")] WorkItemPayload Item);
