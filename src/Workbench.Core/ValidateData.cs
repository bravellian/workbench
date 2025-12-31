namespace Workbench.Core;

/// <summary>
/// Payload describing validation errors, warnings, and counts.
/// </summary>
/// <param name="Errors">Validation errors.</param>
/// <param name="Warnings">Validation warnings.</param>
/// <param name="Counts">Counts of scanned items and docs.</param>
public sealed record ValidateData(
    [property: JsonPropertyName("errors")] IList<string> Errors,
    [property: JsonPropertyName("warnings")] IList<string> Warnings,
    [property: JsonPropertyName("counts")] ValidateCounts Counts);
