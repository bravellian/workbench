namespace Workbench.Core;

/// <summary>
/// Entry describing a work item updated from a GitHub issue.
/// </summary>
/// <param name="ItemId">Work item ID.</param>
/// <param name="IssueUrl">Source issue URL.</param>
public sealed record ItemSyncItemUpdateEntry(
    [property: JsonPropertyName("itemId")] string ItemId,
    [property: JsonPropertyName("issueUrl")] string IssueUrl);
