namespace Workbench.Core;

/// <summary>
/// Abstraction over GitHub API providers (e.g., Octokit or gh CLI).
/// </summary>
public interface IGithubProvider
{
    /// <summary>Checks authentication status for the configured provider.</summary>
    Task<GithubService.AuthStatus> CheckAuthStatusAsync(string repoRoot, string? host = null);
    /// <summary>Ensures the provider is authenticated or throws on failure.</summary>
    Task EnsureAuthenticatedAsync(string repoRoot, string? host = null);
    /// <summary>Fetches a GitHub issue by repo and number.</summary>
    Task<GithubIssue> FetchIssueAsync(string repoRoot, GithubIssueRef issueRef);
    /// <summary>Lists issues for a repository, capped by <paramref name="limit"/>.</summary>
    Task<IList<GithubIssue>> ListIssuesAsync(string repoRoot, GithubRepoRef repo, int limit = 1000);
    /// <summary>Creates a GitHub issue and returns its URL.</summary>
    Task<string> CreateIssueAsync(string repoRoot, GithubRepoRef repo, string title, string body, IEnumerable<string> labels);
    /// <summary>Updates the title and body of a GitHub issue.</summary>
    Task UpdateIssueAsync(string repoRoot, GithubIssueRef issueRef, string title, string body);
    /// <summary>Creates a pull request and returns its URL.</summary>
    Task<string> CreatePullRequestAsync(string repoRoot, GithubRepoRef repo, string title, string body, string? baseBranch, bool draft);
}
