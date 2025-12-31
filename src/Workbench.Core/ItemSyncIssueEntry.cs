namespace Workbench.Core;

/// <summary>
/// Entry describing a created GitHub issue for a work item.
/// </summary>
/// <param name="ItemId">Work item ID.</param>
/// <param name="IssueUrl">URL of the created issue.</param>
public sealed record ItemSyncIssueEntry(
    [property: JsonPropertyName("itemId")] string ItemId,
    [property: JsonPropertyName("issueUrl")] string IssueUrl);
