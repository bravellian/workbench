namespace Workbench.Core;

/// <summary>
/// Count summary for validation output.
/// </summary>
/// <param name="Errors">Number of errors.</param>
/// <param name="Warnings">Number of warnings.</param>
/// <param name="WorkItems">Number of work items scanned.</param>
/// <param name="MarkdownFiles">Number of markdown files scanned.</param>
public sealed record ValidateCounts(
    [property: JsonPropertyName("errors")] int Errors,
    [property: JsonPropertyName("warnings")] int Warnings,
    [property: JsonPropertyName("workItems")] int WorkItems,
    [property: JsonPropertyName("markdownFiles")] int MarkdownFiles);
