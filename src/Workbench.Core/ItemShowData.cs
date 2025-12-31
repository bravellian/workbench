namespace Workbench.Core;

public sealed record ItemShowData(
    [property: JsonPropertyName("item")] WorkItemPayload Item);
