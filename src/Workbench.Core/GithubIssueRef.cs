namespace Workbench.Core;

/// <summary>
/// Lightweight reference to a GitHub issue.
/// </summary>
/// <param name="Repo">Repository reference.</param>
/// <param name="Number">Issue number.</param>
public sealed record GithubIssueRef(GithubRepoRef Repo, int Number);
