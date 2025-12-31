namespace Workbench.Core;

/// <summary>
/// JSON response envelope for the doctor command.
/// </summary>
/// <param name="Ok">True when no errors were detected.</param>
/// <param name="Data">Detailed doctor results.</param>
public sealed record DoctorOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] DoctorData Data);
