namespace Workbench.Core;

public sealed record ItemRenameData(
    [property: JsonPropertyName("item")] WorkItemPayload Item);
