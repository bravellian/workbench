namespace Workbench.Core;

public sealed record NavSyncOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] NavSyncData Data);
