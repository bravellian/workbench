namespace Workbench.Core;

public sealed record ItemSyncOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ItemSyncData Data);
