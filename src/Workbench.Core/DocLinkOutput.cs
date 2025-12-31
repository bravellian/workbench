namespace Workbench.Core;

public sealed record DocLinkOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] DocLinkData Data);
