namespace Workbench.Core;

/// <summary>
/// Payload for work item show output.
/// </summary>
/// <param name="Item">Work item payload.</param>
public sealed record ItemShowData(
    [property: JsonPropertyName("item")] WorkItemPayload Item);
