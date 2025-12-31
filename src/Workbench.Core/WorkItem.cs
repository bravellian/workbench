namespace Workbench.Core;

/// <summary>
/// Parsed work item model loaded from front matter and body content.
/// </summary>
/// <param name="Id">Stable work item identifier.</param>
/// <param name="Type">Work item type (bug, task, spike).</param>
/// <param name="Status">Workflow status string.</param>
/// <param name="Title">Human-readable title.</param>
/// <param name="Priority">Optional priority label.</param>
/// <param name="Owner">Optional owner or assignee.</param>
/// <param name="Created">ISO-like created date string from front matter.</param>
/// <param name="Updated">Optional last-updated date string.</param>
/// <param name="Tags">Tag labels parsed from front matter.</param>
/// <param name="Related">Related links parsed from front matter.</param>
/// <param name="Slug">Slugified title used for filenames.</param>
/// <param name="Path">Absolute path to the work item file.</param>
/// <param name="Body">Markdown body content (without front matter).</param>
public sealed record WorkItem(
    string Id,
    string Type,
    string Status,
    string Title,
    string? Priority,
    string? Owner,
    string Created,
    string? Updated,
    IList<string> Tags,
    RelatedLinks Related,
    string Slug,
    string Path,
    string Body);
