namespace Workbench.Core;

/// <summary>
/// Pairing of an imported issue and the created work item.
/// </summary>
/// <param name="Issue">Imported issue payload.</param>
/// <param name="Item">Created work item payload.</param>
public sealed record ItemImportEntry(
    [property: JsonPropertyName("issue")] GithubIssuePayload Issue,
    [property: JsonPropertyName("item")] WorkItemPayload Item);
