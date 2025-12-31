namespace Workbench.Core;

/// <summary>
/// ID configuration for work items, including numeric formatting and prefixes.
/// </summary>
public sealed record IdsConfig
{
    /// <summary>Numeric width for generated IDs (zero-padded).</summary>
    public int Width { get; init; } = 4;
    /// <summary>Prefix settings for each work item type.</summary>
    public PrefixesConfig Prefixes { get; init; } = new();
}
