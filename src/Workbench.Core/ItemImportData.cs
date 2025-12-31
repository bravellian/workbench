namespace Workbench.Core;

public sealed record ItemImportData(
    [property: JsonPropertyName("items")] IList<ItemImportEntry> Items);
