namespace Workbench.Core;

/// <summary>
/// Payload describing doc deletion results.
/// </summary>
/// <param name="Path">Absolute path to the deleted doc.</param>
/// <param name="ItemsUpdated">Count of work items updated.</param>
public sealed record DocDeleteData(
    [property: JsonPropertyName("path")] string Path,
    [property: JsonPropertyName("itemsUpdated")] int ItemsUpdated);
