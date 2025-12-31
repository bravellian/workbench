namespace Workbench.Core;

public sealed record DoctorOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] DoctorData Data);
