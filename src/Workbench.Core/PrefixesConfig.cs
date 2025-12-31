namespace Workbench.Core;

/// <summary>
/// Prefix values used to generate work item IDs.
/// </summary>
public sealed record PrefixesConfig
{
    /// <summary>Prefix for bug work items.</summary>
    public string Bug { get; init; } = "BUG";
    /// <summary>Prefix for task work items.</summary>
    public string Task { get; init; } = "TASK";
    /// <summary>Prefix for spike work items.</summary>
    public string Spike { get; init; } = "SPIKE";
}
