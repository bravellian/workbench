namespace Workbench.Core;

/// <summary>
/// Optional detail payload for a doctor check.
/// </summary>
/// <param name="Version">Detected version string when applicable.</param>
/// <param name="Error">Error message when the check failed.</param>
/// <param name="Reason">Reason string for non-error statuses.</param>
/// <param name="Path">Path involved in the check.</param>
/// <param name="Missing">Missing paths when path checks fail.</param>
/// <param name="SchemaErrors">Schema validation errors for config or docs.</param>
public sealed record DoctorCheckDetails(
    [property: JsonPropertyName("version")] string? Version,
    [property: JsonPropertyName("error")] string? Error,
    [property: JsonPropertyName("reason")] string? Reason,
    [property: JsonPropertyName("path")] string? Path,
    [property: JsonPropertyName("missing")] IList<string>? Missing,
    [property: JsonPropertyName("schemaErrors")] IList<string>? SchemaErrors);
