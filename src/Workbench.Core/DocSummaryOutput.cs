namespace Workbench.Core;

public sealed record DocSummaryOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] DocSummaryData Data);
