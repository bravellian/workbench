namespace Workbench.Core;

/// <summary>
/// Entry describing an updated GitHub issue for a work item.
/// </summary>
/// <param name="ItemId">Work item ID.</param>
/// <param name="IssueUrl">URL of the updated issue.</param>
public sealed record ItemSyncIssueUpdateEntry(
    [property: JsonPropertyName("itemId")] string ItemId,
    [property: JsonPropertyName("issueUrl")] string IssueUrl);
