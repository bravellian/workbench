namespace Workbench.Core;

/// <summary>
/// JSON response envelope for work item creation.
/// </summary>
/// <param name="Ok">True when the work item was created.</param>
/// <param name="Data">Details about the created work item.</param>
public sealed record ItemCreateOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ItemCreateData Data);
