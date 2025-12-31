namespace Workbench.Core;

/// <summary>
/// Payload describing a newly created work item.
/// </summary>
/// <param name="Id">Assigned work item ID.</param>
/// <param name="Slug">Generated slug for the title.</param>
/// <param name="Path">Absolute path to the work item file.</param>
public sealed record ItemCreateData(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("slug")] string Slug,
    [property: JsonPropertyName("path")] string Path);
