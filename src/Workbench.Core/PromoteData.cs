namespace Workbench.Core;

/// <summary>
/// Payload describing a promote operation.
/// </summary>
/// <param name="Item">Promoted work item payload.</param>
/// <param name="Branch">Created branch name.</param>
/// <param name="Commit">Commit information.</param>
/// <param name="Pushed">True when the branch was pushed.</param>
/// <param name="Pr">Pull request URL when created.</param>
public sealed record PromoteData(
    [property: JsonPropertyName("item")] WorkItemPayload Item,
    [property: JsonPropertyName("branch")] string Branch,
    [property: JsonPropertyName("commit")] CommitInfo Commit,
    [property: JsonPropertyName("pushed")] bool Pushed,
    [property: JsonPropertyName("pr")] string? Pr);
