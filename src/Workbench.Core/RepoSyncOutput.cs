namespace Workbench.Core;

public sealed record RepoSyncOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] RepoSyncData Data);
