namespace Workbench.Core;

/// <summary>
/// Repo-relative paths used by Workbench when creating or locating files.
/// </summary>
public sealed record PathsConfig
{
    /// <summary>Root folder for documentation content.</summary>
    public string DocsRoot { get; init; } = "docs";
    /// <summary>Root folder for work tracking content.</summary>
    public string WorkRoot { get; init; } = "docs/70-work";
    /// <summary>Directory for active work item files.</summary>
    public string ItemsDir { get; init; } = "docs/70-work/items";
    /// <summary>Directory for completed/dropped work item files.</summary>
    public string DoneDir { get; init; } = "docs/70-work/done";
    /// <summary>Directory containing templates for work items and docs.</summary>
    public string TemplatesDir { get; init; } = "docs/70-work/templates";
    /// <summary>Path to the generated workboard index file.</summary>
    public string WorkboardFile { get; init; } = "docs/70-work/README.md";
}
