namespace Workbench.Core;

public sealed record ItemSyncIssueUpdateEntry(
    [property: JsonPropertyName("itemId")] string ItemId,
    [property: JsonPropertyName("issueUrl")] string IssueUrl);
