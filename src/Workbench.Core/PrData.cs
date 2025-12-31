namespace Workbench.Core;

/// <summary>
/// Payload describing a created pull request.
/// </summary>
/// <param name="Pr">Pull request URL.</param>
/// <param name="Item">Work item ID.</param>
public sealed record PrData(
    [property: JsonPropertyName("pr")] string Pr,
    [property: JsonPropertyName("item")] string Item);
