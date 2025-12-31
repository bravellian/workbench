namespace Workbench.Core;

/// <summary>
/// Payload describing normalize command results.
/// </summary>
/// <param name="ItemsUpdated">Number of work items updated.</param>
/// <param name="DocsUpdated">Number of docs updated.</param>
/// <param name="DryRun">True when no files were written.</param>
/// <param name="ItemsNormalized">True when item normalization ran.</param>
/// <param name="DocsNormalized">True when doc normalization ran.</param>
public sealed record NormalizeData(
    [property: JsonPropertyName("itemsUpdated")] int ItemsUpdated,
    [property: JsonPropertyName("docsUpdated")] int DocsUpdated,
    [property: JsonPropertyName("dryRun")] bool DryRun,
    [property: JsonPropertyName("itemsNormalized")] bool ItemsNormalized,
    [property: JsonPropertyName("docsNormalized")] bool DocsNormalized);
