namespace Workbench.Core;

public sealed record ItemDeleteOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ItemDeleteData Data);
