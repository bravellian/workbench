namespace Workbench.Core;

/// <summary>
/// Sync entry describing an imported issue and optional created item.
/// </summary>
/// <param name="Issue">Issue payload.</param>
/// <param name="Item">Created work item payload, or null in dry runs.</param>
public sealed record ItemSyncImportEntry(
    [property: JsonPropertyName("issue")] GithubIssuePayload Issue,
    [property: JsonPropertyName("item")] WorkItemPayload? Item);
