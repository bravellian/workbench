namespace Workbench.Core;

/// <summary>
/// JSON response envelope for work item deletion.
/// </summary>
/// <param name="Ok">True when delete succeeded.</param>
/// <param name="Data">Deleted item payload and backlink stats.</param>
public sealed record ItemDeleteOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ItemDeleteData Data);
