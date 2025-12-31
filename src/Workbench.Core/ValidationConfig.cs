namespace Workbench.Core;

/// <summary>
/// Default validation exclusions for link and doc checks.
/// </summary>
/// <param name="LinkExclude">Repo-relative path prefixes to skip during link validation.</param>
/// <param name="DocExclude">Repo-relative path prefixes to skip during doc validation.</param>
public sealed record ValidationConfig(
    IList<string> LinkExclude,
    IList<string> DocExclude)
{
    /// <summary>
    /// Creates an empty validation config with no exclusions.
    /// </summary>
    public ValidationConfig()
        : this(new List<string>(), new List<string>())
    {
    }
}
