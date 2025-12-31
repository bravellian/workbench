namespace Workbench.Core;

/// <summary>
/// JSON response envelope for GitHub issue import output.
/// </summary>
/// <param name="Ok">True when import succeeded.</param>
/// <param name="Data">Imported items and issue payloads.</param>
public sealed record ItemImportOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ItemImportData Data);
