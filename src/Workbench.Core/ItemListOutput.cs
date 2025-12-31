namespace Workbench.Core;

/// <summary>
/// JSON response envelope for work item list output.
/// </summary>
/// <param name="Ok">True when listing succeeded.</param>
/// <param name="Data">List payload.</param>
public sealed record ItemListOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ItemListData Data);
