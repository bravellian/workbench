namespace Workbench.Core;

/// <summary>
/// Single doctor check result in CLI output.
/// </summary>
/// <param name="Name">Check identifier (e.g., git, config, paths).</param>
/// <param name="Status">Status label (ok/warn/error).</param>
/// <param name="Details">Optional detailed payload for the check.</param>
public sealed record DoctorCheck(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("details")] DoctorCheckDetails? Details);
