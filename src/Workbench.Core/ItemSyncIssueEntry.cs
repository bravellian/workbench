namespace Workbench.Core;

public sealed record ItemSyncIssueEntry(
    [property: JsonPropertyName("itemId")] string ItemId,
    [property: JsonPropertyName("issueUrl")] string IssueUrl);
