namespace Workbench.Core;

public sealed record ItemMoveOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ItemMoveData Data);
