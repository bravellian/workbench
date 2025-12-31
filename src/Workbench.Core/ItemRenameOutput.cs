namespace Workbench.Core;

/// <summary>
/// JSON response envelope for work item rename operations.
/// </summary>
/// <param name="Ok">True when rename succeeded.</param>
/// <param name="Data">Updated work item payload.</param>
public sealed record ItemRenameOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ItemRenameData Data);
