namespace Workbench.Core;

/// <summary>
/// Payload describing doc sync results.
/// </summary>
/// <param name="DocsUpdated">Number of docs updated.</param>
/// <param name="ItemsUpdated">Number of work items updated.</param>
/// <param name="MissingDocs">Referenced docs that could not be found.</param>
/// <param name="MissingItems">Referenced items that could not be found.</param>
public sealed record DocSyncData(
    [property: JsonPropertyName("docsUpdated")] int DocsUpdated,
    [property: JsonPropertyName("itemsUpdated")] int ItemsUpdated,
    [property: JsonPropertyName("missingDocs")] IList<string> MissingDocs,
    [property: JsonPropertyName("missingItems")] IList<string> MissingItems);
