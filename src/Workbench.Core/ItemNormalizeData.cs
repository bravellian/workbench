namespace Workbench.Core;

/// <summary>
/// Payload for work item normalization results.
/// </summary>
/// <param name="ItemsUpdated">Number of items normalized.</param>
/// <param name="DryRun">True when no files were written.</param>
public sealed record ItemNormalizeData(
    [property: JsonPropertyName("itemsUpdated")] int ItemsUpdated,
    [property: JsonPropertyName("dryRun")] bool DryRun);
