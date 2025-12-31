namespace Workbench.Core;

/// <summary>
/// Serializable view of a work item for CLI JSON output.
/// </summary>
/// <param name="Id">Stable work item identifier.</param>
/// <param name="Type">Work item type.</param>
/// <param name="Status">Workflow status.</param>
/// <param name="Title">Human-readable title.</param>
/// <param name="Priority">Optional priority label.</param>
/// <param name="Owner">Optional owner or assignee.</param>
/// <param name="Created">Created date string.</param>
/// <param name="Updated">Updated date string when available.</param>
/// <param name="Tags">Tag labels.</param>
/// <param name="Related">Related links grouped by type.</param>
/// <param name="Slug">Slugified title.</param>
/// <param name="Path">Absolute path to the work item file.</param>
/// <param name="Body">Optional markdown body content.</param>
public sealed record WorkItemPayload(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("priority")] string? Priority,
    [property: JsonPropertyName("owner")] string? Owner,
    [property: JsonPropertyName("created")] string Created,
    [property: JsonPropertyName("updated")] string? Updated,
    [property: JsonPropertyName("tags")] IList<string> Tags,
    [property: JsonPropertyName("related")] RelatedLinksPayload Related,
    [property: JsonPropertyName("slug")] string Slug,
    [property: JsonPropertyName("path")] string Path,
    [property: JsonPropertyName("body")] string? Body);
