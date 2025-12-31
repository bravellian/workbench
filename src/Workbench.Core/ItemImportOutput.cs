namespace Workbench.Core;

public sealed record ItemImportOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ItemImportData Data);
