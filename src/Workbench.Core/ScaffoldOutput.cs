namespace Workbench.Core;

/// <summary>
/// JSON response envelope for scaffold output.
/// </summary>
/// <param name="Ok">True when scaffolding succeeded.</param>
/// <param name="Data">Details about created and skipped paths.</param>
public sealed record ScaffoldOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ScaffoldData Data);
