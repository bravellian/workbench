namespace Workbench.Core;

public sealed record ItemStatusData(
    [property: JsonPropertyName("item")] WorkItemPayload Item);
