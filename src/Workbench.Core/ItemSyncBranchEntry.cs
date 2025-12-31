namespace Workbench.Core;

/// <summary>
/// Entry describing a branch created for a work item.
/// </summary>
/// <param name="ItemId">Work item ID.</param>
/// <param name="Branch">Branch name.</param>
public sealed record ItemSyncBranchEntry(
    [property: JsonPropertyName("itemId")] string ItemId,
    [property: JsonPropertyName("branch")] string Branch);
