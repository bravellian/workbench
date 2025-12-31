namespace Workbench.Core;

public sealed record ItemShowOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ItemShowData Data);
