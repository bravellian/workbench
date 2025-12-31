namespace Workbench.Core;

/// <summary>
/// Payload for work item deletion results.
/// </summary>
/// <param name="Item">Deleted work item payload.</param>
/// <param name="DocsUpdated">Count of docs updated to remove backlinks.</param>
public sealed record ItemDeleteData(
    [property: JsonPropertyName("item")] WorkItemPayload Item,
    [property: JsonPropertyName("docsUpdated")] int DocsUpdated);
