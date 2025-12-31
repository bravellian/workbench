namespace Workbench.Core;

/// <summary>
/// Normalized GitHub issue data used by sync and import workflows.
/// </summary>
/// <param name="Repo">Repository reference for the issue.</param>
/// <param name="Number">Issue number within the repository.</param>
/// <param name="Title">Issue title.</param>
/// <param name="Body">Issue body text.</param>
/// <param name="Url">Canonical issue URL.</param>
/// <param name="State">Issue state (open/closed).</param>
/// <param name="Labels">Label names applied to the issue.</param>
/// <param name="PullRequests">Associated pull request URLs.</param>
public sealed record GithubIssue(
    GithubRepoRef Repo,
    int Number,
    string Title,
    string Body,
    string Url,
    string State,
    IList<string> Labels,
    IList<string> PullRequests);
