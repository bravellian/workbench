namespace Workbench.Core;

/// <summary>
/// Summary projection of a work item for list output.
/// </summary>
/// <param name="Id">Work item ID.</param>
/// <param name="Type">Work item type.</param>
/// <param name="Status">Work item status.</param>
/// <param name="Title">Work item title.</param>
/// <param name="Path">Absolute path to the work item file.</param>
public sealed record ItemSummary(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("path")] string Path);
