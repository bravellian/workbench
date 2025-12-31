namespace Workbench.Core;

/// <summary>
/// JSON response envelope for doc creation.
/// </summary>
/// <param name="Ok">True when the doc was created.</param>
/// <param name="Data">Details about the created doc.</param>
public sealed record DocCreateOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] DocCreateData Data);
