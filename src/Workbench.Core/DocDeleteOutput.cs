namespace Workbench.Core;

/// <summary>
/// JSON response envelope for doc deletion.
/// </summary>
/// <param name="Ok">True when the doc was deleted.</param>
/// <param name="Data">Deletion details.</param>
public sealed record DocDeleteOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] DocDeleteData Data);
