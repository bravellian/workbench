namespace Workbench.Core;

public sealed record ItemSyncItemUpdateEntry(
    [property: JsonPropertyName("itemId")] string ItemId,
    [property: JsonPropertyName("issueUrl")] string IssueUrl);
