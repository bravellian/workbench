namespace Workbench;

public sealed record GithubRepoRef(string Host, string Owner, string Repo)
{
    public string Slug => $"{Owner}/{Repo}";
    public string Display => string.Equals(Host, "github.com", StringComparison.OrdinalIgnoreCase)
        ? Slug
        : $"{Host}/{Slug}";
}

public sealed record GithubIssueRef(GithubRepoRef Repo, int Number);

public sealed record GithubIssue(
    GithubRepoRef Repo,
    int Number,
    string Title,
    string Body,
    string Url,
    string State,
    IList<string> Labels,
    IList<string> PullRequests);
