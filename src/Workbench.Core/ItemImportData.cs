namespace Workbench.Core;

/// <summary>
/// Payload containing imported GitHub issues and created work items.
/// </summary>
/// <param name="Items">Import entries with issue and item payloads.</param>
public sealed record ItemImportData(
    [property: JsonPropertyName("items")] IList<ItemImportEntry> Items);
