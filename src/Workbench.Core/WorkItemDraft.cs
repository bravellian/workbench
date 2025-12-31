namespace Workbench.Core;

/// <summary>
/// AI-generated draft for a work item.
/// </summary>
/// <param name="Title">Suggested title.</param>
/// <param name="Summary">Optional summary text.</param>
/// <param name="AcceptanceCriteria">Optional acceptance criteria list.</param>
/// <param name="Type">Optional suggested type.</param>
/// <param name="Tags">Optional suggested tags.</param>
public sealed record WorkItemDraft(
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("summary")] string? Summary,
    [property: JsonPropertyName("acceptanceCriteria")] IList<string>? AcceptanceCriteria,
    [property: JsonPropertyName("type")] string? Type,
    [property: JsonPropertyName("tags")] IList<string>? Tags);
