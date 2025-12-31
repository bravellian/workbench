namespace Workbench.Core;

public sealed record PrOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] PrData Data);
