namespace Workbench.Core;

/// <summary>
/// JSON response envelope for doc link/unlink operations.
/// </summary>
/// <param name="Ok">True when the operation succeeded.</param>
/// <param name="Data">Link operation details.</param>
public sealed record DocLinkOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] DocLinkData Data);
