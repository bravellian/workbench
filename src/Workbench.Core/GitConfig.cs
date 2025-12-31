namespace Workbench.Core;

/// <summary>
/// Git-related defaults and constraints used by CLI workflows.
/// </summary>
public sealed record GitConfig
{
    /// <summary>Branch name pattern for promoted work items.</summary>
    public string BranchPattern { get; init; } = "work/{id}-{slug}";
    /// <summary>Commit message pattern for promoted work items.</summary>
    public string CommitMessagePattern { get; init; } = "Promote {id}: {title}";
    /// <summary>Default base branch when creating pull requests.</summary>
    public string DefaultBaseBranch { get; init; } = "main";
    /// <summary>When true, workflows that mutate git state require a clean working tree.</summary>
    public bool RequireCleanWorkingTree { get; init; } = true;
}
