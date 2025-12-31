namespace Workbench.Core;

/// <summary>
/// Payload for doctor command JSON output.
/// </summary>
/// <param name="RepoRoot">Resolved repository root.</param>
/// <param name="Checks">List of executed checks.</param>
public sealed record DoctorData(
    [property: JsonPropertyName("repoRoot")] string RepoRoot,
    [property: JsonPropertyName("checks")] IList<DoctorCheck> Checks);
