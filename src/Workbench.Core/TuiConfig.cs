namespace Workbench.Core;

/// <summary>
/// Terminal UI preferences for Workbench.
/// </summary>
public sealed record TuiConfig
{
    /// <summary>Named theme for the TUI.</summary>
    public string Theme { get; init; } = "powershell";
    /// <summary>When true, use emoji labels in the work item list.</summary>
    public bool UseEmoji { get; init; } = true;
    /// <summary>Auto-refresh interval for the TUI in seconds. Set to 0 to disable.</summary>
    public int AutoRefreshSeconds { get; init; } = 60;
}
