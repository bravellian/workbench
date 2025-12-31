namespace Workbench.Core;

/// <summary>
/// JSON response envelope for work item show output.
/// </summary>
/// <param name="Ok">True when the item was resolved.</param>
/// <param name="Data">Work item payload.</param>
public sealed record ItemShowOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ItemShowData Data);
