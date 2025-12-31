namespace Workbench.Core;

/// <summary>
/// JSON response envelope for credential update output.
/// </summary>
/// <param name="Ok">True when update succeeded.</param>
/// <param name="Data">Credential update details.</param>
public sealed record CredentialUpdateOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] CredentialUpdateData Data);
