namespace Workbench.Core;

/// <summary>
/// JSON response envelope for validation output.
/// </summary>
/// <param name="Ok">True when validation produced no errors.</param>
/// <param name="Data">Validation results and counts.</param>
public sealed record ValidateOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ValidateData Data);
