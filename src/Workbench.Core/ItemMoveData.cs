namespace Workbench.Core;

public sealed record ItemMoveData(
    [property: JsonPropertyName("item")] WorkItemPayload Item);
