namespace Workbench.Core;

/// <summary>
/// JSON-friendly representation of a GitHub issue for CLI output.
/// </summary>
/// <param name="Repo">Repository display string.</param>
/// <param name="Number">Issue number.</param>
/// <param name="Url">Issue URL.</param>
/// <param name="Title">Issue title.</param>
/// <param name="State">Issue state.</param>
/// <param name="Labels">Label names.</param>
/// <param name="PullRequests">Associated pull request URLs.</param>
public sealed record GithubIssuePayload(
    [property: JsonPropertyName("repo")] string Repo,
    [property: JsonPropertyName("number")] int Number,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("state")] string State,
    [property: JsonPropertyName("labels")] IList<string> Labels,
    [property: JsonPropertyName("pullRequests")] IList<string> PullRequests);
