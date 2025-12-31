namespace Workbench.Core;

/// <summary>
/// Payload describing workboard regeneration results.
/// </summary>
/// <param name="Path">Path to the regenerated workboard file.</param>
/// <param name="Counts">Counts per status or category.</param>
public sealed record BoardData(
    [property: JsonPropertyName("path")] string Path,
    [property: JsonPropertyName("counts")] IDictionary<string, int> Counts);
