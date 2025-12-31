namespace Workbench.Core;

/// <summary>
/// Payload describing a created document.
/// </summary>
/// <param name="Path">Absolute path to the document.</param>
/// <param name="Type">Document type (spec, adr, runbook, guide, doc).</param>
/// <param name="WorkItems">Linked work item IDs.</param>
public sealed record DocCreateData(
    [property: JsonPropertyName("path")] string Path,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("workItems")] IList<string> WorkItems);
