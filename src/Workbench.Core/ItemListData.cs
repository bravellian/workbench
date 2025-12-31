namespace Workbench.Core;

/// <summary>
/// Payload containing summarized work items.
/// </summary>
/// <param name="Items">Work item summaries.</param>
public sealed record ItemListData(
    [property: JsonPropertyName("items")] IList<ItemSummary> Items);
