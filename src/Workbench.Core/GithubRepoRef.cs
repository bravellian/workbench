namespace Workbench.Core;

/// <summary>
/// Identifies a GitHub repository by host, owner, and name.
/// </summary>
/// <param name="Host">GitHub host (e.g., github.com).</param>
/// <param name="Owner">Repository owner or organization.</param>
/// <param name="Repo">Repository name.</param>
public sealed record GithubRepoRef(string Host, string Owner, string Repo)
{
    /// <summary>Owner/name slug without host.</summary>
    public string Slug => $"{Owner}/{Repo}";
    /// <summary>Display string including host when not github.com.</summary>
    public string Display => string.Equals(Host, "github.com", StringComparison.OrdinalIgnoreCase)
        ? this.Slug
        : $"{Host}/{this.Slug}";
}
